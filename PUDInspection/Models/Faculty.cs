using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Campus Campus { get; set; }
    }
}
