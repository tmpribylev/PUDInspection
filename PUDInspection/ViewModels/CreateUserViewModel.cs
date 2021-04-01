using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Имя человека")]
        public string RealName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Корпоративный адрес")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Ссылка на VK")]
        public string VKLink { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
