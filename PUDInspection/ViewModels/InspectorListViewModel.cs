using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class InspectorListViewModel
    {
        public int InspectionId { get; set; }

        [Display(Name = "Название проверки")]
        public string InspectionName { get; set; }

        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Количество этапов")]
        public int IterationNumber { get; set; }

        [Display(Name = "Текущий этап")]
        public int CurrentIteration { get; set; }

        public bool Hunt { get; set; }
    }
}
