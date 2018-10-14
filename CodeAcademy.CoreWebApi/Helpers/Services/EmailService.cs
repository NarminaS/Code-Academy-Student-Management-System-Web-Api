using CodeAcademy.CoreWebApi.Helpers.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Services
{
    public class EmailService
    {
        private Logger _logger;

        public EmailService(Logger logger)
        {
            _logger = logger;
        }
        public async Task SendEmailAsync(string name, string email, string subject, string message, string requestUrl)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("CodeAcademy Administration", "narmings@code.edu.az"));
                emailMessage.To.Add(new MailboxAddress(name, email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    await client.AuthenticateAsync("narmings@code.edu.az", "taurus91");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "", requestUrl);
            }
        }
    }
}
