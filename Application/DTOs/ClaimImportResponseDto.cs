using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ClaimImportResponseDto
    {
        public int id { get; set; }
        public string file_name { get; set; }
        public string status { get; set; }
        public int total_records { get; set; }
        public int processed_records { get; set; }
        public DateTime created_at { get; set; }
    }
}
