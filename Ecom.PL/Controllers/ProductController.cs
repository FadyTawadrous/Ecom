using Ecom.BLL.ModelVM.Product;
using Ecom.BLL.Responses;
using Ecom.PL.Controllers;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ----------------------- Helper Method -----------------------
        private IActionResult HandleResponse<T>(ResponseResult<T> result)
        {
            if (result == null)
                return StatusCode(500, "Null response from service.");

            // ⭐ Success → 200 OK
            if (result.IsSuccess)
                return Ok(result);

            // ⭐ NotFound cases
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage) &&
                result.ErrorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(result);
            }

            // ⭐ Validation / Business errors → 400 BadRequest
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage) &&
               (result.ErrorMessage.Contains("failed", StringComparison.OrdinalIgnoreCase) ||
                result.ErrorMessage.Contains("invalid", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(result);
            }

            // ⭐ Fallback → Server error
            return StatusCode(500, result);
        }

        // -------------------------------------------------------------

        [HttpGet("all")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var result = await _productService.GetAllForAdminAsync();
            return HandleResponse(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            return HandleResponse(result);
        }

        [HttpGet("brand/{brandId:int}")]
        public async Task<IActionResult> GetAllByBrand(int brandId)
        {
            var result = await _productService.GetAllByBrandIdAsync(brandId);
            return HandleResponse(result);
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetAllByCategory(int categoryId)
        {
            var result = await _productService.GetAllByCategoryIdAsync(categoryId);
            return HandleResponse(result);
        }

        [HttpGet("search/title")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            var result = await _productService.SearchByTitleAsync(title);
            return HandleResponse(result);
        }

        [HttpGet("search/price")]
        public async Task<IActionResult> SearchByPriceRange([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var result = await _productService.SearchByPriceRangeAsync(min, max);
            return HandleResponse(result);
        }

        [HttpGet("search/rating")]
        public async Task<IActionResult> SearchByRating([FromQuery] decimal minRating)
        {
            var result = await _productService.SearchByRatingAsync(minRating);
            return HandleResponse(result);
        }


        // --------------------- Admin Endpoints -----------------------
        //[Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateProductVM model)
        {
            var result = await _productService.CreateAsync(model);
            return HandleResponse(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateProductVM model)
        {
            var result = await _productService.UpdateAsync(model);
            return HandleResponse(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductVM model)
        {
            var result = await _productService.DeleteAsync(model);
            return HandleResponse(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("stock/decrease")]
        public async Task<IActionResult> DecreaseStock(int productId, int quantity)
        {
            var result = await _productService.DecreaseStockAsync(productId, quantity);
            return HandleResponse(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("stock/increase")]
        public async Task<IActionResult> IncreaseStock(int productId, int quantity)
        {
            var result = await _productService.IncreaseStockAsync(productId, quantity);
            return HandleResponse(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("quantity-sold")]
        public async Task<IActionResult> AddToQuantitySold([FromBody] AddQuantitySoldVM model)
        {
            var result = await _productService.AddToQuantitySoldAsync(model);
            return HandleResponse(result);
        }
    }
}
