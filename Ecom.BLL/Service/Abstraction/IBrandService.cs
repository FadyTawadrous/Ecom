using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Ecom.BLL.ModelVM.Brand;

namespace Ecom.BLL.Service.Abstraction
{
    public interface IBrandService
    {
        Task<ResponseResult<IEnumerable<GetBrandVM>>> GetAllAsync(bool includeDeleted = false);
        Task<ResponseResult<GetBrandVM>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> CreateAsync(CreateBrandVM model);
        Task<ResponseResult<bool>> UpdateAsync(UpdateBrandVM model);
        Task<ResponseResult<bool>> DeleteAsync(DeleteBrandVM model);
        Task<ResponseResult<DeleteBrandVM>> GetDeleteModelAsync(int id);
    }
}
