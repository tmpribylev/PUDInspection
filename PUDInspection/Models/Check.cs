using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public abstract class Check
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IterationNumber { get; set; }
        public int CurrentIteration { get; set; }
        public bool Hunt { get; set; }
        public bool Opened { get; set; }
        public bool Closed { get; set; }
        public List<CheckVsCriteria> CriteriaList { get; set; }
        public List<PUD> PUDList { get; set; }
        public List<PUDAllocation> PUDAllocationList { get; set; }
    }
}
