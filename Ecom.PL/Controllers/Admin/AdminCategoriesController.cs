using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.BLL.ModelVM.Category;

namespace Ecom.PL.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : BaseApiController
    {
        private readonly IAdminCategoryService _adminCategoryService;

        public AdminCategoriesController(IAdminCategoryService adminCategoryService)
        {
            _adminCategoryService = adminCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _adminCategoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _adminCategoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddCategoryVM model)
        {
            model.CreatedBy = CurrentUserId;
            var result = await _adminCategoryService.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCategoryVM model)
        {
            model.UpdatedBy = CurrentUserId;
            var result = await _adminCategoryService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _adminCategoryService.SoftDeleteAsync(id, CurrentUserId);
            return Ok(result);
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var result = await _adminCategoryService.HardDeleteAsync(id);
            return Ok(result);
        }
    }
}
