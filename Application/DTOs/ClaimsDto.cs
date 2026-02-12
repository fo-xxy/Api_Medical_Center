using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ClaimsDto
    {
        public int patient_id { get; set; }
        public string claim_number { get; set; }
        public DateOnly service_date { get; set; }
        public Decimal amount { get; set; }
        public String status { get; set; }
        public DateTime created_at { get; set; }

    }

    public class ClaimResponseDto : ClaimsDto
    {
        public int id { get; set; }
    }
}
