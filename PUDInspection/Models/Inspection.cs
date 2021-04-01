using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class Inspection : Check
    {
        public InspectionSpace InspectionSpace { get; set; }
        public List<Validation> Validations { get; set; }
        public List<ApplicationUser> UserList { get; set; }
    }
}
