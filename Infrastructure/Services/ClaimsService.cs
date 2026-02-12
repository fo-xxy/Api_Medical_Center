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


        public async Task<IEnumerable<ClaimsDto>> GetAllAsync()
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
                               created_at = p.created_at
                           })
                           .OrderByDescending(p => p.created_at)
                           .ToListAsync();
        }

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

        public Task<bool> UpdateAsync(int id, ClaimsDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

       

       

       
    }
}
