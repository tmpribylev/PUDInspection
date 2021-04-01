using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class CreateInspectionViewModel
    {
        public int SpaceId { get; set; }

        [Required]
        [Display(Name = "Название проверки")]
        public string InspectionName { get; set; }

        [Required]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Количество этапов")]
        public int IterationNumber { get; set; }

        [Display(Name = "Файл с ПУДами")]
        public IFormFile FilePUD { get; set; }
    }
}
