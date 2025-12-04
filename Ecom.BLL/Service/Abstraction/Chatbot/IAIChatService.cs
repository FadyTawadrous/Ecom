namespace Ecom.BLL.Service.Abstraction.Chatbot
{
    public interface IAIChatService
    {
        Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt);
    }
}
