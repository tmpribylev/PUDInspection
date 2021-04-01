using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDInspection.Models
{
    public class CriteriaEmailText
    {
        public int Id { get; set; }
        public Criteria Criteria { get; set; }
        public EmailText EmailText { get; set; }
        public string Text { get; set; }
    }
}
