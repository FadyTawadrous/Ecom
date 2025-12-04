using Ecom.BLL.Admin.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.PL.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IRoleService _adminRoleService;

        public AdminUsersController(
            IAdminUserService adminUserService,
            IRoleService adminRoleService)
        {
            _adminUserService = adminUserService;
            _adminRoleService = adminRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _adminUserService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
            => Ok(await _adminUserService.GetByIdAsync(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _adminUserService.SoftDeleteAsync(id, adminId));
        }

        // ----- Roles -----

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
            => Ok(await _adminRoleService.GetUserRolesAsync(id));

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateRoles(string id, UserRolesVM model)
        {
            model.UserId = id;
            return Ok(await _adminRoleService.UpdateUserRolesAsync(model));
        }
    }
}
