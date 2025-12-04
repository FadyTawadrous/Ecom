using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.BLL.ModelVM.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/roles")]
    [Authorize(Roles = "Admin")]
    public class AdminRolesController : ControllerBase
    {
        private readonly IRoleService _service;

        public AdminRolesController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllRolesAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleVM model)
            => Ok(await _service.CreateRoleAsync(model));

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> Delete(string roleName)
            => Ok(await _service.DeleteRoleAsync(roleName));
    }
}
