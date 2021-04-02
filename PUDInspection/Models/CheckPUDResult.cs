using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class CheckPUDResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public List<CheckResult> CheckResults { get; set; }
        public ApplicationUser User { get; set; }
        public PUD PUD { get; set; }
        public int Iteration { get; set; }
    }
}
