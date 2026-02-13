using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClaimExportService : IClaimExportService
    {
        private readonly ApplicationDbContext _context;

        public ClaimExportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportClaimsToCsvAsync()
        {
            var claims = await _context.Claims
               .Include(c => c.Patient)
                .OrderByDescending(c => c.created_at)
                .ToListAsync();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var encoding = System.Text.Encoding.GetEncoding(1252);

            var builder = new StringBuilder();
            builder.AppendLine("sep=;");
            builder.AppendLine("claim_number;patient_name;service_date;amount;status");

            foreach (var claim in claims)
            {
                string patientName = $"{claim.Patient.first_name} {claim.Patient.last_name}";
                var format = new NumberFormatInfo { NumberGroupSeparator = ".", NumberDecimalSeparator = "." };
                string total = claim.amount.ToString("N2", format);

                string fechaFromated = $"=\"{claim.service_date:yyyy-MM-dd}\"";

                builder.AppendLine($"{claim.claim_number};{patientName};{fechaFromated};{total};{claim.status}");
            }

            return encoding.GetBytes(builder.ToString());
        }
    }
}
