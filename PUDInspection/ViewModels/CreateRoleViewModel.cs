using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Введите название роли!")]
        [Display(Name = "Название роли")]
        public string RoleName { get; set; }
    }
}
