using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClaimImports
    {
        public int id { get; set; }
        public string file_name { get; set; } = string.Empty;
        public int total_records { get; set; }
        public int processed_records { get; set; }
        public string status { get; set; } = "pending";
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
