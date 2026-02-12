using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClaimsService
    {
        Task<IEnumerable<ClaimsDto>> GetAllAsync();
        Task<ClaimResponseDto> RegisterAsync(ClaimsDto dto);
        Task<bool> UpdateAsync(int id, ClaimsDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
