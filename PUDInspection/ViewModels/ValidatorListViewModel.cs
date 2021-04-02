using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class ValidatorListViewModel
    {
        public int ValidationId { get; set; }

        [Display(Name = "Название перепроверки")]
        public string ValidationName { get; set; }

        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Количество этапов")]
        public int IterationNumber { get; set; }

        [Display(Name = "Текущий этап")]
        public int CurrentIteration { get; set; }

        public bool Hunt { get; set; }

        public bool Opened { get; set; }
    }
}
