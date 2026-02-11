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
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Servicio para obtener todos los pacientes
        public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
        {
            return await _context.Patients
                .Select(p => new PatientResponseDto
                {
                    id = p.id,
                    first_name = p.first_name,
                    last_name = p.last_name,
                    dob = p.dob,
                    created_at = p.created_at
                })
                .OrderByDescending(p => p.created_at)
                .ToListAsync();
        }

        //Servicio para registrar un nuevo paciente
        public async Task<PatientResponseDto> RegisterAsync(PatientDto dto)
        {
            var patient = new Patients
            {
                first_name = dto.first_name,
                last_name = dto.last_name,
                dob = dto.dob.Value,
                created_at = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return new PatientResponseDto
            {
                id = patient.id        
            };
        }

        //Servicio para actualizar la información de un paciente
        public async Task<bool> UpdateAsync(int id, PatientDto dto)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null) return false; 

            patient.first_name = dto.first_name;
            patient.last_name = dto.last_name;
            patient.dob = dto.dob ?? patient.dob; 

            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            return true;
        }

        //Servicio para eliminar un paciente
        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
