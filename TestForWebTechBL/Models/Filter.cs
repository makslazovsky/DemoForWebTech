using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForWebTechBL.Models
{
    public class Filter
    {
        public string PropertyOrderName {  get; set; }
        public bool PropertyOrder { get; set; }
        public string SearchText { get; set; }
        public string PropertySearchName { get; set; }
        public int PageNumber { get; set; }
    }
}
