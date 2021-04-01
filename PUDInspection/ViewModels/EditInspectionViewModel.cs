using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PUDInspection.Models;

namespace PUDInspection.ViewModels
{
    public class EditInspectionViewModel
    {
        public int SpaceId { get; set; }

        public int InspectionId { get; set; }

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

        public List<Criteria> Criterias { get; set; }
        public List<string> Users { get; set; }

        [Display(Name = "Файл с ПУДами")]
        public IFormFile FilePUD { get; set; }
    }
}
