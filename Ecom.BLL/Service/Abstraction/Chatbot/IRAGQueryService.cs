namespace Ecom.BLL.Service.Abstraction.Chatbot
{
    public interface IRAGQueryService
    {
        Task<List<(Product Product, double Score)>> SearchAsync(string query, int topK = 5);
    }
}
