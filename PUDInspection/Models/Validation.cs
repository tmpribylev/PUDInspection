using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class Validation : Check
    {
        public Inspection Inspection { get; set; }
        public List<ApplicationUser> UserList { get; set; }
    }
}
