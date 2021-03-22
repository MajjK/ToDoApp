using System.Net.Mail;

namespace ToDoApp.Services
{
    public class EmailProfile
    {
        private string ServerEmail = "example@gmail.com";
        private string ServerEmailPassword = "example";

        public bool SendEmail(string userEmail, string confirmationLink, string subject)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ServerEmail);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(ServerEmail, ServerEmailPassword),
                Host = "smtp.gmail.com",
                Port = 587,
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
