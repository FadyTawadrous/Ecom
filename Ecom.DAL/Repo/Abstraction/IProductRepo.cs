
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IProductRepo
    {
        // Query Methods
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(
            Expression<Func<Product, bool>>? filter = null,
            params Expression<Func<Product, object>>[] includes);

        // Command Methods
        Task<bool> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> ToggleDeleteStatusAsync(int id, string userModified);

        Task<bool> DecreaseStockAsync(int productId, int quantity);
        Task<bool> IncreaseStockAsync(int productId, int quantity);

        Task<bool> UpdateRatingAsync(int productId, decimal newAverageRating);

        Task<bool> AddToQuantitySoldAsync(int productId, int quantity);
    }
}
