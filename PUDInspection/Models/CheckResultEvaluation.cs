using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class CheckResultEvaluation
    {
        public int Id { get; set; }
        public int Evaluation { get; set; }
        public PUD PUD { get; set; }
        public ApplicationUser User { get; set; }
        public Validation Validation { get; set; }
    }
}
