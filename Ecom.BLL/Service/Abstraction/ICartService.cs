
namespace Ecom.BLL.Service.Abstraction
{
    public interface ICartService
    {
        Task<ResponseResult<GetCartVM>> GetByUserIdAsync(string UserId);
        Task<ResponseResult<GetCartVM>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> AddAsync(AddCartVM model);
        Task<ResponseResult<bool>> ClearCartAsync(int cartId);
    }
}