using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
    }
}
