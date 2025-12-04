
using Ecom.BLL.Admin.Service.Abstraction;

namespace Ecom.BLL.Admin.Service.Implementation
{
    public class AdminBrandService : IAdminBrandService
    {
        private readonly IBrandService _brandService;

        public AdminBrandService(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public Task<ResponseResult<IEnumerable<GetBrandVM>>> GetAllAsync(bool includeDeleted = false)
        {
            return _brandService.GetAllAsync(includeDeleted);
        }

        public Task<ResponseResult<GetBrandVM>> GetByIdAsync(int id)
        {
            return _brandService.GetByIdAsync(id);
        }

        public Task<ResponseResult<bool>> CreateAsync(CreateBrandVM model)
        {
            // CreatedBy will be set in controller
            return _brandService.CreateAsync(model);
        }

        public Task<ResponseResult<bool>> UpdateAsync(UpdateBrandVM model)
        {
            // UpdatedBy will be set in controller
            return _brandService.UpdateAsync(model);
        }

        public Task<ResponseResult<bool>> DeleteAsync(int id, string deletedBy)
        {
            var vm = new DeleteBrandVM
            {
                Id = id,
                DeletedBy = deletedBy
            };

            return _brandService.DeleteAsync(vm);
        }
    }
}
