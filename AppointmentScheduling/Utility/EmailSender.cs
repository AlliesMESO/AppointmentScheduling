using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace AppointmentScheduling.Utility
{
    public class EmailSender : IEmailSenders
    {
        private IConfiguration Configuration;
        public EmailSender(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            string host = this.Configuration.GetValue<string>("Smtp:Server");
            int port = this.Configuration.GetValue<int>("Smtp:Port");
            string fromAddress = this.Configuration.GetValue<string>("Smtp:FromAddress");
            string userName = this.Configuration.GetValue<string>("Smtp:UserName");
            string password = this.Configuration.GetValue<string>("Smtp:Password");

            try
            {
                using (MailMessage mm = new MailMessage(fromAddress, email))
                {
                    mm.Subject = subject;
                    mm.Body = htmlMessage;
                    mm.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(host,port))
                    {
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(userName, password);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Send(mm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
