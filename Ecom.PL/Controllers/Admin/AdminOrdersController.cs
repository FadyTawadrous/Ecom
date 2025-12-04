using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.DAL.Enum;

namespace Ecom.PL.Controllers.Admin
{
    [Route("api/admin/orders")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : BaseApiController
    {
        private readonly IAdminOrderService _adminOrderService;

        public AdminOrdersController(IAdminOrderService adminOrderService)
        {
            _adminOrderService = adminOrderService;
        }

        // GET: api/admin/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _adminOrderService.GetAllAsync();
            return Ok(response); // { isSuccess, result, errorMessage }
        }

        // GET: api/admin/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _adminOrderService.GetByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
                return NotFound(response);

            return Ok(response);
        }

        // GET: api/admin/orders/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var response = await _adminOrderService.GetByUserIdAsync(userId);
            return Ok(response);
        }

        // PUT: api/admin/orders/{id}/status?status=Shipped
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] OrderStatus status)
        {
            var response = await _adminOrderService.UpdateStatusAsync(id, status, CurrentUserId);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // PUT: api/admin/orders/{id}/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var response = await _adminOrderService.CancelOrderAsync(id, CurrentUserId);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // DELETE: api/admin/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _adminOrderService.DeleteAsync(id, CurrentUserId);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
