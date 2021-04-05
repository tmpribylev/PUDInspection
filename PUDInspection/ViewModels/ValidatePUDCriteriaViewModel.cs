using PUDInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class ValidatePUDCriteriaViewModel
    {
        public int CheckVsCriteriaId { get; set; }
        public Criteria Criteria { get; set; }
        public int CheckResult { get; set; }
    }
}
