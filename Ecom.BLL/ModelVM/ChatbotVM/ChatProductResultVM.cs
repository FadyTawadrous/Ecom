
namespace Ecom.BLL.ModelVM.ChatbotVM
{
    public class ChatProductResultVM
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public double Score { get; set; }
    }
}
