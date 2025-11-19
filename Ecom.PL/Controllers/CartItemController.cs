using Ecom.BLL.ModelVM.CartItem;
using Ecom.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _service;

        public CartItemController(ICartItemService service)
        {
            _service = service;
        }

        [HttpGet("cart/{cartId:int}")]
        public async Task<IActionResult> GetByCartId(int cartId)
        {
            var result = await _service.GetByCartIdAsync(cartId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("cart/{cartId:int}/product/{productId:int}")]
        public async Task<IActionResult> GetByCartIdAndProductId(int cartId, int productId)
        {
            var result = await _service.GetByCartIdAndProductIdAsync(cartId, productId);

            if (!result.IsSuccess)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCartItemVM model)
        {
            var result = await _service.AddAsync(model);

            if (!result.IsSuccess)
                return Conflict("Could not add cart item");

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCartItemVM model)
        {
            var result = await _service.UpdateAsync(model);

            if (!result.IsSuccess)
                return NotFound("Cart item not found");

            return Ok(result);
        }

        [HttpPatch("toggle-delete")]
        public async Task<IActionResult> ToggleDelete(DeleteCartItemVM model)
        {
            var result = await _service.DeleteAsync(model);

            if (!result.IsSuccess)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var result = await _service.HardDeleteAsync(new DeleteCartItemVM { Id = id });

            if (!result.IsSuccess)
                return NotFound();

            return NoContent();
        }
    }
}
