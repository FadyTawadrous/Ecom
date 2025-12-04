namespace Ecom.BLL.Service.Abstraction.Chatbot
{
    public interface IProductIndexingService
    {
        Task<int> buildAllEmbeddingsAsync(string createdBy);
        Task<bool> buildEmbeddingForProductAsync(int productId, string createdBy);
    }
}
