using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClaimImportService : IClaimImportService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _baseUploadPath = "claims_uploads/imports";

        public ClaimImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClaimImportResponseDto> UploadAsync(IFormFile file)
        {
            string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
            string targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), _baseUploadPath, dateFolder);

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }


            string uniqueFileName = $"claims_import_{Guid.NewGuid().ToString().Substring(0, 8)}_{file.FileName}";
            string fullPath = Path.Combine(targetDirectory, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var importRecord = new ClaimImports
            {
                file_name = uniqueFileName,
                status = "pending",
                total_records = 0,
                processed_records = 0,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            _context.ClaimImports.Add(importRecord);
            await _context.SaveChangesAsync();


            await this.ImportAsync(importRecord.id, fullPath);

            return new ClaimImportResponseDto
            {
                id = importRecord.id,
                file_name = importRecord.file_name,
                status = importRecord.status,
                total_records = importRecord.total_records,
                processed_records = importRecord.processed_records,
                created_at = importRecord.created_at
            };
        }

        public async Task ImportAsync(int importId, string filePath)
        {
            var import = await _context.ClaimImports.FindAsync(importId);
            if (import == null) return;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                List<string> lines = new List<string>();

                using (var reader = new StreamReader(filePath, Encoding.GetEncoding(1252)))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lines.Add(line);
                    }
                }

                if (lines.Count <= 1) return;

                var dataline = lines.Skip(1).ToList();
                import.total_records = dataline.Count;
                import.status = "processing";
                await _context.SaveChangesAsync();

                foreach (var line in dataline)
                {
                    var column = line.Split(';');
                    if (column.Length < 7) continue;

                    string firstName = column[0].Trim();
                    string lastName = column[1].Trim();
                    string patient_dob = column[2].Trim();
                    if (!DateOnly.TryParse(patient_dob, out DateOnly patientDobDate))
                    {
                        continue;
                    }
                    string claimNum = column[3].Trim();

                    if (!DateOnly.TryParse(column[4].Trim(), out DateOnly serviceDate)) continue;

                    string amountStr = column[5].Trim();

                    string cleanAmount = amountStr.Replace(".", "").Replace(",", ".");
                    if (!decimal.TryParse(cleanAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal claimAmount))
                    {
                        claimAmount = 0;
                    }

                    string csvStatus = column[6].Trim();

                    var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.first_name.ToLower() == firstName.ToLower()
                                          && p.last_name.ToLower() == lastName.ToLower());

                    bool claimExists = await _context.Claims.AnyAsync(c => c.claim_number == claimNum);

                    if (!claimExists)
                    {
                        if (patient == null)
                        {
                            patient = new Patients
                            {
                                first_name = firstName,
                                last_name = lastName,
                                dob = patientDobDate,
                                created_at = DateTime.UtcNow
                            };

                            _context.Patients.Add(patient);

                            await _context.SaveChangesAsync();
                        }

                        var newClaim = new Claims
                        {
                            patient_id = patient.id,
                            claim_import_id = import.id,
                            claim_number = claimNum,
                            service_date = serviceDate,
                            amount = claimAmount,
                            status = !string.IsNullOrEmpty(csvStatus) ? csvStatus : "Pending",
                            created_at = DateTime.UtcNow
                        };

                        _context.Claims.Add(newClaim);
                        import.processed_records++;

                    }
                }
                    

                import.status = "completed";
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                import.status = "failed";
            }
            finally
            {
                import.updated_at = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}