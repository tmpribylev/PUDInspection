using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class InspectionPUDResult : CheckPUDResult
    {
        public List<CheckResultEvaluation> CheckResultEvaluations { get; set; }
        public Inspection Inspection { get; set; }
    }
}
