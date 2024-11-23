using ExpenseTracker.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ExpenseTracker.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _password;
        private readonly string _email;
        public EmailSender(IConfiguration config)
        {
            _email = config["Email"];
            _password = config["Password"];
            
        }
        public void SendEmail(string toEmail, string subject)
        {
            

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_email, _password);

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("abby.wehner53@ethereal.email");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>User Registered</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>Thank you For Registering account</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}
