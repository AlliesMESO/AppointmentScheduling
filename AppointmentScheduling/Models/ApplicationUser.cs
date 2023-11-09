using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentScheduling.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Identification { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Workplace { get; set; } 
        [MaxLength(100)] // maximum length of the address if needed
        public string Address { get; set; }
        public string ProfilePicturePath { get; set; }
        public string Gender { get; set; }
    }
}
