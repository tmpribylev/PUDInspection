using PUDInspection.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class InspectPUDCriteriaViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int CheckVsCriteriaId { get; set; }
        public Criteria Criteria { get; set; }
        public int CheckResult { get; set; }
    }
}
