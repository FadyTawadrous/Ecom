using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : BaseApiController
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserId()
        {
            var result = await _service.GetByUserIdAsync(CurrentUserId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]AddCartVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(model);

            if (!result.IsSuccess)
                return Conflict(result);

            return Ok(result);
        }

        [HttpDelete("clear/{cartId}")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var result = await _service.ClearCartAsync(cartId);

            if (!result.IsSuccess)
                return NotFound(result);

            return NoContent();
        }
    }
}
