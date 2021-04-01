using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        [Display(Name = "Имя человека")]
        public string RealName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Корпоративная почта")]
        public string Email { get; set; }

        [Display(Name = "Адрес подтвержден?")]
        public bool EmailComfirmed { get; set; }

        [Required]
        [Display(Name = "Ссылка VK")]
        public string VKLink { get; set; }

        public IList<string> Roles { get; set; }
    }
}
