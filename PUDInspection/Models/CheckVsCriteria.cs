using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class CheckVsCriteria
    {
        public int Id { get; set; }
        public Criteria Criteria { get; set; }
        public Check Check { get; set; }
        public List<CheckResult> CheckResults { get; set; }
    }
}
