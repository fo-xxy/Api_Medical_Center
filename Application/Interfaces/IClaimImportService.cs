using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IClaimImportService
    {
        Task<ClaimImportResponseDto> UploadAsync(IFormFile file);

        Task ImportAsync(int importId, string filePath);
    }
}
