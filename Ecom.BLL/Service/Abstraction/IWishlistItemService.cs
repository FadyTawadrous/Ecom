
namespace Ecom.BLL.Service.Abstraction
{
    public interface IWishlistItemService
    {
        // Get
        Task<ResponseResult<GetWishlistItemVM>> GetByIdAsync(int id);
        Task<ResponseResult<PaginatedResult<GetWishlistItemVM>>> GetAllAsync(string? searchName = null, 
            int pageNumber = 1, int pageSize = 10);
        Task<ResponseResult<PaginatedResult<GetWishlistItemVM>>> GetAllByUserIdAsync(string userId,
            string? searchName = null, int pageNumber = 1, int pageSize = 10);

        // Create
        Task<ResponseResult<bool>> CreateAsync(CreateWishlistItemVM model);

        // Delete
        Task<ResponseResult<bool>> DeleteAsync(DeleteWishlistItemVM model);

        // Get Delete Model
        Task<ResponseResult<DeleteWishlistItemVM>> GetDeleteModelAsync(int id);
    }
}
