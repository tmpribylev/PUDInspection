using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class Criteria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxValue { get; set; }
        public bool AutoCheck { get; set; }
        public string AutoCheckFormula { get; set; }
        public int Important { get; set; }
        public bool Deleted { get; set; }
    }
}
