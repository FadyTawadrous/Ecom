using Ecom.BLL.ModelVM.ChatbotVM;
using Ecom.BLL.Service.Abstraction.Chatbot;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RAGController : ControllerBase
    {
        private readonly IProductIndexingService _indexingService;
        private readonly IRAGQueryService _ragQueryService;
        private readonly IProductRagService _ragService;

        public RAGController(
            IProductIndexingService indexingService,
            IRAGQueryService ragQueryService,
            IProductRagService ragService)
        {
            _indexingService = indexingService;
            _ragQueryService = ragQueryService;
            _ragService = ragService;
        }

        // ================================
        // ADMIN ENDPOINT - BUILD EMBEDDINGS
        // ================================
        [HttpPost("reindex")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reindex()
        {
            var user = User.Identity?.Name ?? "system";
            var count = await _indexingService.buildAllEmbeddingsAsync(user);

            return Ok(new
            {
                Message = $"Embeddings generated for {count} products."
            });
        }

        // ================================
        // DEBUG SEARCH (SIMILARITY SCORE)
        // ================================
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, int k = 5)
        {
            var results = await _ragQueryService.SearchAsync(q, k);

            return Ok(results.Select(r => new
            {
                ProductId = r.Product.Id,
                Product = r.Product.Title,
                Score = r.Score
            }));
        }

        // ================================
        // PRODUCTION CHATBOT ENDPOINT
        // ================================
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequestVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Message))
                return BadRequest("Message is required");

            var result = await _ragService.AnswerAsync(model.Message, model.TopK);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
