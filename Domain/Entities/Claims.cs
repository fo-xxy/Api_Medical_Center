using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Claims
    {
        public int id { get; set; }
        public int patient_id { get; set; }

        public int claim_import_id { get; set; }

        public string claim_number { get; set; }
        public DateOnly service_date { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; } = "pending";
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public Patients Patient { get; set; }
        public ClaimImports ClaimImport { get; set; }

    }
}
