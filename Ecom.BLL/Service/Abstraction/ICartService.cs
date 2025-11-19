
using Ecom.BLL.ModelVM.Cart;

namespace Ecom.BLL.Service.Abstraction
{
    public interface ICartService
    {
        Task<ResponseResult<GetCartVM>> GetByUserIdAsync(string UserId);
        Task<ResponseResult<bool>> AddAsync(AddCartVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateCartVM model);
        Task<ResponseResult<bool>> DeleteAsync(DeleteCartVM model);
        Task<ResponseResult<bool>> HardDeleteAsync(DeleteCartVM model);
    }
}