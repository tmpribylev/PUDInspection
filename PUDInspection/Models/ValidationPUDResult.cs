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
    }
}
