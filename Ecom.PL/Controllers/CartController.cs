using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await _service.GetByUserIdAsync(userId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCartVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(model);

            if (!result.IsSuccess)
                return Conflict(result);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCartVM model)
        {
            var result = await _service.UpdateAsync(model);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("toggle-delete")]
        public async Task<IActionResult> ToggleDelete(DeleteCartVM model)
        {
            var result = await _service.DeleteAsync(model);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var result = await _service.HardDeleteAsync(new DeleteCartVM { Id = id });

            if (!result.IsSuccess)
                return NotFound(result);

            return NoContent();
        }
    }
}
