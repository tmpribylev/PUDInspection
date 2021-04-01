using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Faculty Faculty { get; set; }
    }
}
