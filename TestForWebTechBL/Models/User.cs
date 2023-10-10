using System.ComponentModel.DataAnnotations;

namespace TestForWebTechBl.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

    }
}
