
namespace Ecom.BLL.Service.Abstraction
{
    public interface ICartReminderService
    {
        Task SendAbandonedCartEmailsAsync();
    }
}
