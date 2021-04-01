using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class EmailText
    {
        public int Id { get; set; }
        public string GoodPUDText { get; set; }
        public string StartText { get; set; }
        public string EndText { get; set; }
        public string DescriptionText { get; set; }
        public List<CriteriaEmailText> CriteriaEmailTextList { get; set; }
    }
}
