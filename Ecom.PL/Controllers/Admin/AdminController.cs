using Ecom.BLL.Admin.Service.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboard;

        public AdminController(IAdminDashboardService dashboard)
        {
            _dashboard = dashboard;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Get()
        {
            var data = await _dashboard.GetOverviewAsync();
            return Ok(data);
        }
    }
}
