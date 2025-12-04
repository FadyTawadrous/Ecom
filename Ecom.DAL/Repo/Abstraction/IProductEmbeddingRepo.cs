
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IProductEmbeddingRepo
    {
        Task<ProductEmbedding?> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductEmbedding>> GetAllAsync();
        Task<bool> AddAsync(ProductEmbedding embedding);
        Task<bool> UpdateAsync(ProductEmbedding embedding);
        Task<bool> DeleteByProductIdAsync(int productId);
        Task<bool> DeleteAllAsync();
    }
}
