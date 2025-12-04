
using Ecom.BLL.ModelVM.ChatbotVM;

namespace Ecom.BLL.Service.Abstraction.Chatbot
{
    public interface IProductRagService
    {
        Task<ResponseResult<ChatResponseVM>> AnswerAsync(string question, int topK = 5);
    }
}
