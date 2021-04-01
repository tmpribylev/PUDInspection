using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class InspectionSpace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Closed { get; set; }
        public List<ApplicationUser> UserList { get; set; }
        public List<Inspection> InspectionList { get; set; }
    }
}
