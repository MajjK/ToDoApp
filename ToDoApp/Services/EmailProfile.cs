using System;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ToDoApp.Services
{
    public static class EmailProfile
    {
        public static IConfiguration Configuration;

        public static bool SendEmail(string userEmail, string confirmationLink, string subject)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Configuration["EmailConfig:ServerEmail"]);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(Configuration["EmailConfig:ServerEmail"],
                    Configuration["EmailConfig:ServerEmailPassword"]),
                Host = Configuration["EmailConfig:Host"],
                Port = Int32.Parse(Configuration["EmailConfig:Port"]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 20000
            };

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
