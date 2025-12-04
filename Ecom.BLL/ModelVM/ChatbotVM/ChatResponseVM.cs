
namespace Ecom.BLL.ModelVM.ChatbotVM
{
    public class ChatResponseVM
    {
        public string Answer { get; set; } = string.Empty;
        public List<ChatProductResultVM> Products { get; set; } = new();
    }
}
