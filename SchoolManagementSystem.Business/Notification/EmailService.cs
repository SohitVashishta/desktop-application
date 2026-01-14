using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Notification
{
    public class EmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("you@gmail.com", "APP_PASSWORD"),
                EnableSsl = true
            };

            var mail = new MailMessage("you@gmail.com", to, subject, body);
            await smtp.SendMailAsync(mail);
        }
    }
}
