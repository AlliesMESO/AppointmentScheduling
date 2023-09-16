using System;

namespace AppointmentScheduling.Models.ViewModels
{
    public class UserProfileVM
    {
        // Other properties...
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } // Add this property for the phone number
        public string Workplace { get; set; }
        public int Age { get; set; } // Add Age property
        public string Gender { get; set; } // Add Gender property
        public string Address { get; set; }
        public string ProfilePicturePath { get; set; }

        // Add other properties here if needed.
    }
}
