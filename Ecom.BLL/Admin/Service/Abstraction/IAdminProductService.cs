
using Ecom.BLL.ModelVM.Product;

namespace Ecom.BLL.Admin.Service.Abstraction
{
    public interface IAdminProductService
    {
        Task<ResponseResult<IEnumerable<GetProductVM>>> GetAllAsync();
        Task<ResponseResult<bool>> CreateAsync(CreateProductVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateProductVM model);
        Task<ResponseResult<bool>> DeleteAsync(int id);
        Task<ResponseResult<bool>> IncreaseStockAsync(int productId, int quantity);
        Task<ResponseResult<bool>> DecreaseStockAsync(int productId, int quantity);
    }
}
