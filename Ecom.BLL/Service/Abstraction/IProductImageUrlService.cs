
namespace Ecom.BLL.Service.Abstraction
{
    public interface IProductImageUrlService
    {
        Task<ResponseResult<IEnumerable<GetProductImageUrlVM>>> GetAllAsync();
        Task<ResponseResult<GetProductImageUrlVM>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> CreateAsync(CreateProductImageUrlVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateProductImageUrlVM model);
        Task<ResponseResult<bool>> DeleteAsync(DeleteProductImageUrlVM model);
    }
}
