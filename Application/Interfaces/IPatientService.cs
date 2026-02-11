using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDto>> GetAllAsync();
        Task<PatientResponseDto> RegisterAsync(PatientDto dto);
        Task<bool> UpdateAsync(int id, PatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
