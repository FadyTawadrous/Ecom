using Ecom.BLL.ModelVM.Order;
using Ecom.BLL.ModelVM.OrderStatusVM;
using Ecom.BLL.Service.Abstraction;
using Ecom.DAL.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await orderService.GetByIdAsync(Id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        //Get All Orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await orderService.GetAllAsync();

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await orderService.GetOrdersByUserIdAsync(userId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        // CREATE ORDER (Creates order From Cart of userId)
        // ==========================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromQuery] string shippingAddress)
        {
            if (CurrentUserId == null)
                return Unauthorized("User Not Logged in.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await orderService.CreateOrderAsync(CurrentUserId, shippingAddress);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // DELETE ORDER (Soft Delete)
        // ==========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> ToggleDelete(int id)
        {
            if (CurrentUserId == null)
                return Unauthorized("User Not Logged in.");

            var result = await orderService.DeleteAsync(id, CurrentUserId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusVM model)
        {
            var result = await orderService.UpdateStatusAsync(id, model.NewStatus, model.UpdatedBy);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (CurrentUserId == null)
                return Unauthorized("User Not Logged in.");
            var result = await orderService.CancelOrderAsync(id,CurrentUserId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
