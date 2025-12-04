using Ecom.BLL.Service.Abstraction.Chatbot;

namespace Ecom.BLL.Service.Implementation.Chatbot
{
    // Service to handle product indexing and embedding creation
    // Uses Nomic AI embedding service to generate embeddings for products
    public class ProductIndexingService : IProductIndexingService
    {
        private readonly IProductRepo _productRepo;
        private readonly IEmbeddingService _embeddingService;
        private readonly IProductEmbeddingRepo _productEmbeddingRepo;
        private readonly IConfiguration _configuration;
        public ProductIndexingService(IProductRepo productRepo, IEmbeddingService embeddingService, IProductEmbeddingRepo productEmbeddingRepo, IConfiguration configuration)
        {
            _productRepo = productRepo;
            _embeddingService = embeddingService;
            _productEmbeddingRepo = productEmbeddingRepo;
            _configuration = configuration;
        }

        // Build embeddings for all products
        // Returns the number of embeddings created
        // Uses Nomic AI embedding service  
        public async Task<int> buildAllEmbeddingsAsync(string createdBy)
        {
            try
            {
                // 1) Clear existing embeddings
                await _productEmbeddingRepo.DeleteAllAsync();

                // 2) Get all non-deleted products with related data
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted,
                    p => p.Brand, p => p.Category, p => p.ProductReviews
                );

                // build embeddings
                // return count of embeddings created
                // Note: In a production scenario, consider batching and parallel processing for efficiency
                // Also, handle rate limiting and errors from the embedding service
                int count = 0;
                var modelName = _configuration["NomicAI:EmbeddingModel"] ?? "nomic-embed-text-v1.5";

                // 3) Iterate over products and create embeddings
                // Note: This is a simple sequential implementation. For large datasets, consider batching and parallel processing.
                // Also, implement error handling and retry logic for robustness.
                foreach (var p in products)
                {
                    // a- Build the text to be embedded
                    var text = BuildEmbeddingText(p);

                    // b- Generate the embedding vector
                    var vector = await _embeddingService.GenerateEmbeddingAsync(text);

                    // c- Convert the vector to byte array
                    var bytes = VectorConverter.DoubleArrayToBytes(vector);

                    // d- Create and save the ProductEmbedding entity
                    var embedding = new ProductEmbedding(p.Id, text, bytes, createdBy, modelName);

                    // e- Save to repository
                    await _productEmbeddingRepo.AddAsync(embedding);

                    // f- Increment count
                    count++;
                }

                return count;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Build embedding for a single product by ID
        public async Task<bool> buildEmbeddingForProductAsync(int productId, string createdBy)
        {
            // 1) Check if embedding already exists
            // If exists, delete it to rebuild
            var productEmbedding = await _productEmbeddingRepo.GetByProductIdAsync(productId);
            if (productEmbedding != null)
                await _productEmbeddingRepo.DeleteByProductIdAsync(productId);

            // 2) Get the product by ID
            // If not found or deleted, return false
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null || product.IsDeleted)
                return false;

            // 3) Build the text to be embedded
            var text = BuildEmbeddingText(product);

            // 4) Generate the embedding vector
            var vector = await _embeddingService.GenerateEmbeddingAsync(text);

            // 5) Convert the vector to byte array
            var bytes = VectorConverter.DoubleArrayToBytes(vector);

            // Get model name from configuration
            var modelName = _configuration["NomicAI:EmbeddingModel"] ?? "nomic-embed-text-v1.5";

            // 6) delete existing embedding for the product
            await _productEmbeddingRepo.DeleteByProductIdAsync(productId);

            // 7) Create and save the ProductEmbedding entity
            var embedding = new ProductEmbedding(productId, text, bytes, createdBy, modelName);
            return await _productEmbeddingRepo.AddAsync(embedding);
        }

        // Helper method to build the text representation of a product for embedding
        private string BuildEmbeddingText(Product p)
        {
            // Include product details and reviews in the text to be embedded 
            var reviewsText = p.ProductReviews != null && p.ProductReviews.Any()
                ? string.Join(" | ", p.ProductReviews.Select(r => $"{r.Title}: {r.Description} (Rating: {r.Rating})"))
                : "No reviews";

                    return $@"
                                Product: {p.Title}
                                Brand: {p.Brand?.Name}
                                Category: {p.Category?.Name}
                                Description: {p.Description}
                                Price: {p.Price}
                                Discount: {p.DiscountPercentage}
                                Rating: {p.Rating}

                                Reviews:
                                {reviewsText}
                            ";
        }
    }
}
