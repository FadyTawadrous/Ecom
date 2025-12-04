using Ecom.BLL.ModelVM.ProductReview;

namespace Ecom.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductReviewController : BaseApiController
    {
        private readonly IProductReviewService _reviewService;

        public ProductReviewController(IProductReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // --------------------------------------------------------
        // 1) GET: api/ProductReview
        // --------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _reviewService.GetAllAsync();
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 2) GET: api/ProductReview/product/{productId}
        // --------------------------------------------------------
        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var response = await _reviewService.GetByProductIdAsync(productId);
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 3) GET: api/ProductReview/user/{userId}
        // --------------------------------------------------------
        [Authorize]
        [HttpGet("user/")]
        public async Task<IActionResult> GetByUserId()
        {

            var response = await _reviewService.GetByUserIdAsync(CurrentUserId);
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 4) GET: api/ProductReview/brand/{brandId}
        // --------------------------------------------------------
        [HttpGet("brand/{brandId:int}")]
        public async Task<IActionResult> GetByBrandId(int brandId)
        {
            var response = await _reviewService.GetByBrandIdAsync(brandId);
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 5) GET: api/ProductReview/{id}
        // --------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _reviewService.GetByIdAsync(id);
            if (!response.IsSuccess) return NotFound(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 6) POST: api/ProductReview
        // --------------------------------------------------------
        //[Authorize]// → If user should be logged in
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductReviewCreateVM model)
        {
            var response = await _reviewService.CreateAsync(CurrentUserId, model);
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 7) PUT: api/ProductReview
        // --------------------------------------------------------
        //[Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductReviewUpdateVM model)
        {
            var response = await _reviewService.UpdateAsync(CurrentUserId,model);
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }

        // --------------------------------------------------------
        // 8) PATCH: api/ProductReview/toggle/{id}
        // --------------------------------------------------------
        //[Authorize]
        [HttpPatch("toggle/{id:int}")]
        public async Task<IActionResult> ToggleDelete(int id)
        {
            // Usually userModified comes from token → User.Identity.Name
            string? user = User?.Identity?.Name ?? "system";

            var response = await _reviewService.ToggleDeleteAsync(id, user);
            if (!response.IsSuccess) return BadRequest(response);

            return Ok(response);
        }
    }
}
