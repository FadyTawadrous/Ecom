using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ecom.BLL.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var client = new SendGridClient(_emailConfig.ApiKey);
            var from = new EmailAddress(_emailConfig.FromEmail, _emailConfig.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"Email sent to {toEmail} with status code {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }

    }
}
