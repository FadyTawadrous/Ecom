using Ecom.BLL.ModelVM.ProductImageURL;
using Ecom.BLL.Responses;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageUrlController : BaseApiController
    {
        private readonly IProductImageUrlService _imageService;

        public ProductImageUrlController(IProductImageUrlService imageService)
        {
            _imageService = imageService;
        }

        // ----------------------------
        // 1️⃣ Get All Product Images
        // ----------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _imageService.GetAllAsync();
            return HandleResponse(result);
        }

        // ----------------------------
        // 2️⃣ Get Product Image by ID
        // ----------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _imageService.GetByIdAsync(id);
            return HandleResponse(result);
        }

        // ----------------------------
        // 3️⃣ Create Product Image
        // Only Admin
        // ----------------------------
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateProductImageUrlVM model)
        {
            var result = await _imageService.CreateAsync(model);
            return HandleResponse(result);
        }

        // ----------------------------
        // 4️⃣ Update Product Image
        // Only Admin
        // ----------------------------
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateProductImageUrlVM model)
        {
            var result = await _imageService.UpdateAsync(model);
            return HandleResponse(result);
        }

        // ----------------------------
        // 5️⃣ Delete / Toggle Product Image
        // Only Admin
        // ----------------------------
        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductImageUrlVM model)
        {
            var result = await _imageService.DeleteAsync(model);
            return HandleResponse(result);
        }

        // ----------------------------
        // Helper method to map ResponseResult<T> to IActionResult
        // ----------------------------
        private IActionResult HandleResponse<T>(ResponseResult<T> response)
        {
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}

