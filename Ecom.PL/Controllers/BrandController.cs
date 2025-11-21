
namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseApiController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            this._brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _brandService.GetAllAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Result); // return Enumerable of GetBrandVM
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var result = await _brandService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Result); // return GetBrandVM
            }
            return NoContent();
        }
        [Authorize]
        [HttpPost]
        
        public async Task<IActionResult> Create([FromForm] CreateBrandVM model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _brandService.CreateAsync(model);
                if (Response.IsSuccess)
                {
                    return NoContent();
                }
                ModelState.AddModelError(string.Empty, Response.ErrorMessage);
                return BadRequest(Response.ErrorMessage);
            }
            return BadRequest("Invalid data.");
        }



        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateBrandVM model)
        {
            if (id != model.Id)
            {
                return BadRequest("ID mismatch.");
            }
            if (ModelState.IsValid)
            {
                var response = await _brandService.UpdateAsync(model);
                if (response.IsSuccess)
                {
                    return NoContent();
                }
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                return BadRequest(response.ErrorMessage);
            }
            return BadRequest("Invalid data.");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //this Delete Action is for soft delete
            //validate Id
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            var BrandToBeDeleted = await _brandService.GetDeleteModelAsync(id); //fetch data from db
            if (!BrandToBeDeleted.IsSuccess)
            {
                return NotFound();
            }
            var response = await _brandService.DeleteAsync(BrandToBeDeleted.Result);// delete from Db
            if (response.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(response.ErrorMessage);

        }
    }
}