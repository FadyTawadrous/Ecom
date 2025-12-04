
namespace Ecom.BLL.Admin.Service.Abstraction
{
    public interface IAdminCategoryService
    {
        Task<ResponseResult<IEnumerable<GetCategoryVM>>> GetAllAsync();
        Task<ResponseResult<GetCategoryVM>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> CreateAsync(AddCategoryVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateCategoryVM model);
        Task<ResponseResult<bool>> SoftDeleteAsync(int id, string deletedBy);
        Task<ResponseResult<bool>> HardDeleteAsync(int id);
    }
}
