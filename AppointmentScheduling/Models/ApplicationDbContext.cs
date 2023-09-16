using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppointmentScheduling.Models; // Make sure this is the correct namespace for your ApplicationUser


namespace AppointmentScheduling.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Define your DbSet properties for your entities (e.g., Appointments)

        public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<AnotherEntity> AnotherEntities { get; set; }

        // Other DbSet properties for your entities
        public DbSet<ApplicationUser> User { get; set; } //Name it 'User/Users'

    }
}
