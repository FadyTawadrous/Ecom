
using Ecom.BLL.Admin.Service.Abstraction;

namespace Ecom.BLL.Admin.Service.Implementation
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryService(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public Task<ResponseResult<IEnumerable<GetCategoryVM>>> GetAllAsync()
        {
            // just forward – CategoryService already filters IsDeleted
            return _categoryService.GetAllAsync();
        }

        public Task<ResponseResult<GetCategoryVM>> GetByIdAsync(int id)
        {
            return _categoryService.GetByIdAsync(id);
        }

        public Task<ResponseResult<bool>> CreateAsync(AddCategoryVM model)
        {
            // CreatedBy will be filled in controller (CurrentUserId)
            return _categoryService.AddAsync(model);
        }

        public Task<ResponseResult<bool>> UpdateAsync(UpdateCategoryVM model)
        {
            // UpdatedBy will be filled in controller
            return _categoryService.UpdateAsync(model);
        }

        public Task<ResponseResult<bool>> SoftDeleteAsync(int id, string deletedBy)
        {
            var vm = new DeleteCategoryVM
            {
                Id = id,
                DeletedBy = deletedBy
            };

            return _categoryService.DeleteAsync(vm);
        }

        public Task<ResponseResult<bool>> HardDeleteAsync(int id)
        {
            var vm = new DeleteCategoryVM
            {
                Id = id
            };

            return _categoryService.HardDeleteAsync(vm);
        }
    }
}
