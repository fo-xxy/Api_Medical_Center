using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly ApplicationDbContext _context;

        public ClaimsService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Servicio para obtener todos los reclamos
        public async Task<IEnumerable<ClaimResponseDto>> GetAllAsync()
        {
            return await _context.Claims
                           .Select(p => new ClaimResponseDto
                           {
                               id = p.id,
                               patient_id = p.patient_id,
                               claim_number = p.claim_number,
                               service_date = p.service_date,
                               amount = p.amount,
                               status = p.status,
                               created_at = p.created_at,

                           })
                           .OrderByDescending(p => p.created_at)
                           .ToListAsync();
        }


        public async Task<IEnumerable<ClaimListResponseDto>> GetAllWithNameAsync()
        {
            return await _context.Claims
                           .Select(p => new ClaimListResponseDto
                           {
                               id = p.id,
                               patient_id = p.patient_id,
                               patient_name = p.Patient.first_name + " " + p.Patient.last_name,
                               claim_number = p.claim_number,
                               service_date = p.service_date,
                               amount = p.amount,
                               status = p.status,
                               created_at = p.created_at,
                               idImport = p.claim_import_id ?? 0
                           })
                           .OrderByDescending(p => p.created_at)
                           .ToListAsync();
        }

        public async Task<IEnumerable<ClaimListImportResponseDto>> GetAllHistoryAsync()
        {
            return await _context.ClaimImports
                           .Select(p => new ClaimListImportResponseDto
                           {
                               id = p.id,
                               file_name = p.file_name,
                               processed_records = p.processed_records,
                               status = p.status,
                               created_at = p.created_at
                           })
                           .OrderByDescending(p => p.created_at)
                           .ToListAsync();
        }

        //Servicio para registrar un nuevo reclamo
        public async Task<ClaimResponseDto> RegisterAsync(ClaimsDto dto)
        {
            var claim = new Claims

            {
                patient_id = dto.patient_id,
                claim_number = dto.claim_number,
                service_date = dto.service_date,
                amount = dto.amount,
                status = dto.status,
                created_at = DateTime.UtcNow
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return new ClaimResponseDto
            {
                id = claim.id
            };
        }

        //Servicio para actualizar un reclamo
        public async Task<bool> UpdateAsync(int id, ClaimsDto dto)
        {
            var claim = await _context.Claims.FindAsync(id);

            if(claim == null) return false;

            claim.patient_id = dto.patient_id;
            claim.claim_number = dto.claim_number;
            claim.service_date = dto.service_date;
            claim.amount = dto.amount;
            claim.status = dto.status;
            claim.updated_at = DateTime.UtcNow;

            _context.Claims.Update(claim);

            await _context.SaveChangesAsync();

            return true;

        }

        //Servicio para eliminar un reclamo
        public async Task<bool> DeleteAsync(int id)
        {
            var claim = await _context.Claims.FindAsync(id);

            if (claim == null) return false;

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
