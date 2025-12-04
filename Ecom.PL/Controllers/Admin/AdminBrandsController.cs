using Ecom.BLL.Admin.Service.Abstraction;

namespace Ecom.PL.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/brands")]
    [Authorize(Roles = "Admin")]
    public class AdminBrandsController : BaseApiController
    {
        private readonly IAdminBrandService _adminBrandService;

        public AdminBrandsController(IAdminBrandService adminBrandService)
        {
            _adminBrandService = adminBrandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var result = await _adminBrandService.GetAllAsync(includeDeleted);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _adminBrandService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBrandVM model)
        {
            model.CreatedBy = CurrentUserId;
            var result = await _adminBrandService.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateBrandVM model)
        {
            model.UpdatedBy = CurrentUserId;
            var result = await _adminBrandService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _adminBrandService.DeleteAsync(id, CurrentUserId);
            return Ok(result);
        }
    }
}
