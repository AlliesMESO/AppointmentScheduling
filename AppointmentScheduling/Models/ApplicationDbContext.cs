using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppointmentScheduling.Models; // Make sure this is the correct namespace for your ApplicationUser
using AppointmentScheduling.Models.ViewModels;

namespace AppointmentScheduling.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Define your DbSet properties for your entities (e.g., Appointments)

        public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<AnotherEntity> AnotherEntities { get; set; }

        // Other DbSet properties for your entities
        public DbSet<ApplicationUser> User { get; set; } //Name it 'User/Users'
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Your customizations

            //Configure the chat entity.
            builder.Entity<ChatMessage>()
                .ToTable("Chats")
                .HasKey(c => c.MessageId);

                // Add other configurations here if needed.

            base.OnModelCreating(builder);

        }

        public DbSet<ChatMessage> Chats { get; set; }

    }
}
