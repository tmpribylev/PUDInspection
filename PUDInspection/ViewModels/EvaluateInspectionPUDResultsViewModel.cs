using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class EvaluateInspectionPUDResultsViewModel
    {
        public string InspectionPUDResultId { get; set; }

        public int Evaluation { get; set; }

        public List<int> CheckResultEvaluations { get; set; }

        public List<string> CriteriaNames { get; set; }
    }
}
