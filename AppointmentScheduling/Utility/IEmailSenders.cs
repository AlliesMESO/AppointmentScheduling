using System.Threading.Tasks;

namespace AppointmentScheduling.Utility
{
    public interface IEmailSenders
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);


    }
}
