﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class ValidatePUDViewModel
    {
        public string UserId { get; set; }
        public int ValidationID { get; set; }
        public int CurrentIteration { get; set; }
        public int AllocationId { get; set; }
        public int PUDId { get; set; }
        public string Link { get; set; }
        public string EduProgram { get; set; }
        public string Department { get; set; }
        public string EducationStage { get; set; }
        public string Language { get; set; }
        public string CourseName { get; set; }
        public string Details { get; set; }
        public List<ValidatePUDCriteriaViewModel> Criterias { get; set; }
        public List<EvaluateInspectionPUDResultsViewModel> InspectionResults { get; set; }
    }
}
