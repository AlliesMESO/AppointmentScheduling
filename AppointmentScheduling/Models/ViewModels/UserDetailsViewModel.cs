using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduling.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public string UserId { get; set; }
        public string VerificationCode { get; set; }

        [Required(ErrorMessage = "Provide your phone number.")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Provide your age")]
        [Range(10, 300, ErrorMessage = "You must be between 10 and 300 years to progress")]
        public int Age { get; set; }

        public enum Gender
        {
            Male,
            Female,
            Other
        }

        [Required(ErrorMessage = "Gender specification must be declared.")]
        public Gender UserGender { get; set; }

        [Required(ErrorMessage = "A valid ID number is required")]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }
        public string Identification { get; set; }
    }
}
