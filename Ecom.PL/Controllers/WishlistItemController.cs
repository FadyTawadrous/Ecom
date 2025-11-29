
namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistItemController : BaseApiController
    {
        private readonly IWishlistItemService _wishlistItemService;

        public WishlistItemController(IWishlistItemService wishlistItemService)
        {
            _wishlistItemService = wishlistItemService;
        }

        // Get Operations
        [Authorize(Roles = "Admin")]
        [HttpGet("WishlistItems")]
        public async Task<IActionResult> GetAll([FromQuery] string? name = null, 
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _wishlistItemService.GetAllAsync(name, pageNum, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data                
            }
            return NotFound(result.ErrorMessage); // 404
        }

        [HttpGet("WishlistItems/{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var result = await _wishlistItemService.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data
            }
            return NotFound(result.ErrorMessage); // 404            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Users/{userId}/WishlistItems")]
        public async Task<IActionResult> GetAllByUserId([FromRoute] string userId, [FromQuery] string? itemName = null,
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _wishlistItemService.GetAllByUserIdAsync(userId, itemName, pageNum, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data                
            }
            return NotFound(result.ErrorMessage); // 404
        }

        [HttpGet("Users/me/WishlistItems")]
        public async Task<IActionResult> GetAllForCurrentUser([FromQuery] string? itemName = null,
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _wishlistItemService.GetAllByUserIdAsync(CurrentUserId, itemName, pageNum, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data                
            }
            return NotFound(result.ErrorMessage); // 404
        }

        // Create Operation
        [HttpPost("WishlistItems")]
        public async Task<IActionResult> Create([FromBody] CreateWishlistItemVM model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUserId;
                model.AppUserId = CurrentUserId;

                var result = await _wishlistItemService.CreateAsync(model);
                if (result.IsSuccess)
                {
                    return NoContent(); // 204
                }

                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest(result.ErrorMessage); // 400
            }
            return BadRequest("Invalid data."); // 400
        }

        // Delete Operation
        [HttpDelete("WishlistItems/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID."); // 400
            }

            var itemToBeDeleted = await _wishlistItemService.GetDeleteModelAsync(id); //fetch data from db
            if (!itemToBeDeleted.IsSuccess)
            {
                return NotFound(); // 404
            }

            var response = await _wishlistItemService.DeleteAsync(itemToBeDeleted.Result);// delete from Db
            if (response.IsSuccess)
            {
                return NoContent(); // 204
            }

            return BadRequest(response.ErrorMessage); // 400
        }
    }
}
