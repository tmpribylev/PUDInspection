using Microsoft.AspNetCore.Identity;
using PUDInspection.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string RealName { get; set; }
        public string VKLink { get; set; }
        public bool Blocked { get; set; }
        public List<Inspection> Inspections { get; set; }
        public List<Validation> Validations { get; set; }
        public List<PUDAllocation> PUDAllocations { get; set; }
        public List<InspectionPUDResult> InspectionPUDResults { get; set; }
        public List<ValidationPUDResult> ValidationPUDResults { get; set; }
        public List<InspectionSpace> InspectionSpaces { get; set; }
        public List<CheckResultEvaluation> CheckResultEvaluations { get; set; }
    }
}
