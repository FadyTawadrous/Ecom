
namespace Ecom.BLL.Service.Abstraction
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
    }
}
