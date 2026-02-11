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
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<bool> RegisterAsync(UserRegisterDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.email == dto.email);
            if (exists) return false;

            var user = new Users
            {
                email = dto.email,
                password_digest = (dto.password_digest),
                role = dto.role,
                created_at = DateTime.UtcNow
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();

           return result > 0;
        }
    }
}