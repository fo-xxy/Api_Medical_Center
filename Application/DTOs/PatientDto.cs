using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PatientDto
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateOnly? dob { get; set; }
    }

    public class PatientResponseDto : PatientDto
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
    }
}
