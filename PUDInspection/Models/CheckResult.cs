using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class CheckResult
    {
        public int Id { get; set; }
        public int Evaluation { get; set; }
        public PUD PUD { get; set; }
        public ApplicationUser User { get; set; }
        public CheckVsCriteria InspectionCriteria { get; set; }
        public int Iteration { get; set; }
    }
}
