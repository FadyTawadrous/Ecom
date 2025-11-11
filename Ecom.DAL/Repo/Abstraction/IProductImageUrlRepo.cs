
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IProductImageUrlRepo
    {
        Task<ProductImageUrl?> GetByIdAsync(int id);
        Task<IEnumerable<ProductImageUrl>> GetAllAsync(
            Expression<Func<ProductImageUrl, bool>>? Filter = null,
            params Expression<Func<ProductImageUrl, object>>[] includes);

        // Command Methods
        Task<bool> AddAsync(ProductImageUrl newImage);
        Task<bool> UpdateAsync(ProductImageUrl newImage);
        Task<bool> ToggleDeleteStatusAsync(int id, string userModified);
    }
}
