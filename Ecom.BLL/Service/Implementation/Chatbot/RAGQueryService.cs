using Ecom.BLL.Service.Abstraction.Chatbot;

namespace Ecom.BLL.Service.Implementation.Chatbot
{
    // Service to handle RAG (Retrieval-Augmented Generation) queries for products
    // Uses embedding service to generate query embeddings and compares with product embeddings
    // to find the most relevant products
    public class RAGQueryService : IRAGQueryService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IProductEmbeddingRepo _embeddingRepo;

        public RAGQueryService(
            IEmbeddingService embeddingService,
            IProductEmbeddingRepo embeddingRepo)
        {
            _embeddingService = embeddingService;
            _embeddingRepo = embeddingRepo;
        }

        // Search for products relevant to the query using embeddings
        // Returns a list of products with their similarity scores
        // The higher the score, the more relevant the product is to the query
        public async Task<List<(Product Product, double Score)>> SearchAsync(string query, int topK = 5)
        {
            // 1) Generate embedding for the query string which is provided by the user
            var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(query);

            // 2) Retrieve all product embeddings from the repository
            var allEmbeddings = await _embeddingRepo.GetAllAsync();

            // Prepare a list to hold ranked products
            // Tuple of Product and its similarity Score
            var ranked = new List<(Product Product, double Score)>();

            // 3) Calculate similarity scores and rank products
            // Using Cosine Similarity to measure relevance
            foreach (var emb in allEmbeddings)
            {
                // a- Get the associated product for the embedding 
                var product = emb.Product;
                if (product == null || product.IsDeleted)
                    continue;

                // b- Convert stored byte array back to double array for similarity calculation
                var productVector = VectorConverter.BytesToDoubleArray(emb.Vector);

                // c- Calculate cosine similarity between query embedding ( By User ) and product embedding (Stored data)
                var score = MathCalculation.CosineSimilarity(queryEmbedding, productVector);

                // d- Add to ranked list
                ranked.Add((product, score));
            }

            // 4) Return the top K products sorted by similarity score in descending order
            return ranked
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .ToList();
        }
    }
}
