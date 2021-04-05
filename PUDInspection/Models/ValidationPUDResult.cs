using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class ValidationPUDResult : CheckPUDResult
    {
        public List<InspectionPUDResult> InspectionPUDResults { get; set; }
        public Validation Validation { get; set; }
        public List<CheckResult> CheckResults { get; set; }
        public ApplicationUser User { get; set; }
        public PUD PUD { get; set; }
    }
}
