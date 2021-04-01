using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class PUDChange
    {
        public int Id { get; set; }
        public PUD PUD { get; set; }
        public DateTime Date { get; set; }
        public string Changes{ get; set; }
    }
}
