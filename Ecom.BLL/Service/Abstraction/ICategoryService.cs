
using Ecom.BLL.ModelVM.Category;
using Ecom.DAL.Entity;
using System.Linq.Expressions;

namespace Ecom.BLL.Service.Abstraction
{
    public interface ICategoryService
    {
        Task<ResponseResult<IEnumerable<GetCategoryVM>>> GetAllAsync();
        Task<ResponseResult<GetCategoryVM>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> AddAsync(AddCategoryVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateCategoryVM model);
        Task<ResponseResult<bool>> DeleteAsync(DeleteCategoryVM model);
        Task<ResponseResult<bool>> HardDeleteAsync(DeleteCategoryVM model);
    }
}
