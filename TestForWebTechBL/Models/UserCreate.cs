using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestForWebTechBl.Models;

namespace TestForWebTechBL.Models
{
    public class UserCreate
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public List<int> UserRoleIds { get; set; }
    }
}
