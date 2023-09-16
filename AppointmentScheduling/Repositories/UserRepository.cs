using AppointmentScheduling.Models;
using Mailjet.Client.Resources;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentScheduling.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Implement interface methods here
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateUserProfile(ApplicationUser user)
        {
            // Update user profile logic here
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // Other methods as needed

        // Ensure that you implement all methods defined in the IUserRepository interface
    }
}
