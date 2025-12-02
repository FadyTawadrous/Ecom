
using Ecom.BLL.Responses;

namespace Ecom.DAL.Repo.Abstraction
{
    public interface IWishlistItemRepo
    {
        // Query Methods
        Task<WishlistItem?> GetByIdAsync(int id, 
            params Expression<Func<WishlistItem, object>>[] includes);
        Task<PaginatedResult<WishlistItem>> GetAllByUserIdAsync(string userId,
            Expression<Func<WishlistItem, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<WishlistItem, object>>[] includes);
        Task<PaginatedResult<WishlistItem>> GetAllAsync(
            Expression<Func<WishlistItem, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<WishlistItem, object>>[] includes);

        // Command Methods
        Task<bool> AddAsync(WishlistItem newItem);
        Task<bool> DeleteAsync(int id);
    }
}
