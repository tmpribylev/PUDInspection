using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class PUDAllocation
    {
        public int Id { get; set; }
        public int Iteration { get; set; }
        public bool Checked { get; set; }
        public Inspection Inspection { get; set; }
        public ApplicationUser User { get; set; }
        public PUD PUD { get; set; }
    }
}
