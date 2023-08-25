using Microsoft.AspNetCore.Identity;

namespace AppointmentScheduling.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Identification { get; set; }
        public int Age { get; set; }
    }
}
