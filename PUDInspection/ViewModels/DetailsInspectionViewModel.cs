using PUDInspection.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class DetailsInspectionViewModel
    {
        public int SpaceId { get; set; }

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

        public bool Closed { get; set; }

        public bool Opened { get; set; }

        public bool Hunt { get; set; }

        public int PUDCount { get; set; }

        public int CurrentCheckPUDCount { get; set; }

        public List<CheckVsCriteria> Criterias { get; set; }

        public List<string> Users { get; set; }

        public List<string> Validations { get; set; }
    }
}
