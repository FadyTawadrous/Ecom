
namespace Ecom.BLL.ModelVM.ChatbotVM
{
    public class ChatRequestVM
    {
        public string Message { get; set; } = string.Empty;
        public int TopK { get; set; } = 5;
    }
}
