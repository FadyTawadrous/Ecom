
namespace Ecom.BLL.Service.Abstraction
{
    public interface ICartItemService
    {
        Task<ResponseResult<IEnumerable<GetCartItemVM>>> GetByCartIdAsync(int cartId);
        Task<ResponseResult<GetCartItemVM>> GetByIdAsync(int id);
        Task<ResponseResult<GetCartItemVM>> GetByCartIdAndProductIdAsync(int cartId, int productId);

        Task<ResponseResult<bool>> AddAsync(AddCartItemVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateCartItemVM model);
        Task<ResponseResult<bool>> DeleteAsync(int id);
    }
}
    