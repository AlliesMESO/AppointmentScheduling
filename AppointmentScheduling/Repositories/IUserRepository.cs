using AppointmentScheduling.Models;
using Mailjet.Client.Resources;
using System.Collections.Generic;

namespace AppointmentScheduling.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetUserById(string userId);
        IEnumerable<ApplicationUser> GetAllUsers();
        void UpdateUserProfile(ApplicationUser user);
        // Other interface methods
    }
}
