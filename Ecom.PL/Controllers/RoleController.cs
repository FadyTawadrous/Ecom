
namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            return Ok(await _roleManager.Roles.ToListAsync());
        }

        [HttpPost("crole")]
        public async Task<IActionResult> Create([FromBody] string roleVM)
        {

            var getRoleByName = await _roleManager.FindByNameAsync(roleVM); //Admin
            if (getRoleByName is not { })
            {
                var role = new IdentityRole() { Name = roleVM };
                var result = await _roleManager.CreateAsync(role);
                return Ok("Successfully created role");
            }
            return Ok("Role already exist");
        }
    }
}
