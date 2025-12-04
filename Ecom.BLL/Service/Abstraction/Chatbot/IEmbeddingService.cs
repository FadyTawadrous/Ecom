namespace Ecom.BLL.Service.Abstraction.Chatbot
{
    public interface IEmbeddingService
    {
        Task<double[]> GenerateEmbeddingAsync(string inputText);
    }
}
