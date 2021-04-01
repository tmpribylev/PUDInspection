using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class CreateValidationViewModel
    {
        public int InspId { get; set; }

        [Required]
        [Display(Name = "Название перепроверки")]
        public string ValidationName { get; set; }

        [Required]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Количество этапов")]
        public int IterationNumber { get; set; }
    }
}
