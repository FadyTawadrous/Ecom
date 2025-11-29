
namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // Get Operations
        [Authorize(Roles = "Admin")]
        [HttpGet("Addresses")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _addressService.GetAllAsync(pageNum, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data                
            }
            return NotFound(result.ErrorMessage); // 404
        }

        [HttpGet("Addresses/{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var result = await _addressService.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data
            }
            return NotFound(result.ErrorMessage); // 404            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Users/{userId}/Addresses")]
        public async Task<IActionResult> GetAllByUserId([FromRoute] string userId,
            [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _addressService.GetAllByUserIdAsync(userId, pageNum, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data                
            }
            return NotFound(result.ErrorMessage); // 404
        }

        [HttpGet("Users/me/Addresses")]
        public async Task<IActionResult> GetAllForCurrentUser([FromQuery] int pageNum = 1, 
            [FromQuery] int pageSize = 10)
        {
            var result = await _addressService.GetAllByUserIdAsync(CurrentUserId, pageNum, pageSize);

            if (result.IsSuccess)
            {
                return Ok(result.Result); // 200 with data                
            }
            return NotFound(result.ErrorMessage); // 404
        }

        // Create Operation
        [HttpPost("Addresses")]
        public async Task<IActionResult> Create([FromBody] CreateAddressVM model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUserId;
                model.AppUserId = CurrentUserId;

                var result = await _addressService.CreateAsync(model);
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
            return BadRequest("Invalid data.");
        }

        // Update Operation
        [HttpPut("Addresses/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressVM model)
        {
            if (id != model.Id)
            {
                return BadRequest("ID mismatch.");
            }
            if (ModelState.IsValid)
            {
                model.UpdatedBy = CurrentUserId;

                var response = await _addressService.UpdateAsync(model);
                if (response.IsSuccess)
                {
                    return NoContent();
                }
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                return BadRequest(response.ErrorMessage);
            }
            return BadRequest("Invalid data.");
        }

        // Delete Operation
        [HttpDelete("Addresses/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //validate Id
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            var addressToBeDeleted = await _addressService.GetDeleteModelAsync(id); //fetch data from db
            if (!addressToBeDeleted.IsSuccess)
            {
                return NotFound();
            }
            addressToBeDeleted.Result.DeletedBy = CurrentUserId;

            var response = await _addressService.DeleteAsync(addressToBeDeleted.Result);// delete from Db
            if (response.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(response.ErrorMessage);
        }
    }
}