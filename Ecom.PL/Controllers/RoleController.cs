
namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // All role management endpoints require Admin role
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Roles management
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleVM>>> GetAllRoles()
        {
            var response = await _roleService.GetAllRolesAsync();
            
            return Ok(response.Result);
        }

        [HttpPost("roles")]
        public async Task<ActionResult<RoleVM>> CreateRole([FromBody] CreateRoleVM createRoleVM)
        {
            var response = await _roleService.CreateRoleAsync(createRoleVM);

            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetAllRoles), new { id = response.Result?.Id }, response.Result);
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<ActionResult<bool>> DeleteRole(string roleName)
        {
            var response = await _roleService.DeleteRoleAsync(roleName);

            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.ErrorMessage });
            }

            return Ok(response.Result);
        }

        // User role management
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string userId)
        {
            var response = await _roleService.GetUserRolesAsync(userId);

            if (!response.IsSuccess)
            {
                return NotFound(new { message = response.ErrorMessage });
            }

            return Ok(response.Result);
        }

        [HttpPost("user/update")]
        public async Task<ActionResult<bool>> UpdateUserRoles([FromBody] UserRolesVM userRolesVM)
        {
            var response = await _roleService.UpdateUserRolesAsync(userRolesVM);

            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.ErrorMessage });
            }

            return Ok(response.Result);
        }

    }
}
