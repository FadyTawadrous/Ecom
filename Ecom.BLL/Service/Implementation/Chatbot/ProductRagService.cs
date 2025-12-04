
using Ecom.BLL.ModelVM.ChatbotVM;
using Ecom.BLL.Service.Abstraction.Chatbot;

namespace Ecom.BLL.Service.Implementation.Chatbot
{
    // Service to handle RAG (Retrieval-Augmented Generation) for product-related questions
    // Combines retrieval of relevant products with AI-generated answers
    // Uses IRAGQueryService to find relevant products and IAIChatService to generate answers
    // based on the retrieved products
    // Returns a structured response containing the answer and relevant product details
    public class ProductRagService : IProductRagService
    {
        private readonly IRAGQueryService _ragQueryService;
        private readonly IAIChatService _aIChatService;
        private readonly IProductRepo _productRepo;

        public ProductRagService(
            IRAGQueryService ragQueryService,
            IProductRepo productRepo,
            IAIChatService aIChatService)
        {
            _ragQueryService = ragQueryService;
            _productRepo = productRepo;
            _aIChatService = aIChatService;
        }

        // AnswerAsync: Generate an answer to a product-related question using RAG approach
        public async Task<ResponseResult<ChatResponseVM>> AnswerAsync(string question, int topK = 5)
        {
            // 1) Retrieve relevant products using RAG query service
            // The products are ranked based on their relevance to the question
            var ranked = await _ragQueryService.SearchAsync(question, topK);

            // If no relevant products found, return a response indicating that
            if (!ranked.Any())
            {
                return new ResponseResult<ChatResponseVM>(
                    null,
                    "No relevant products found.",
                    false
                );
            }


            var products = new List<(Product Product, double Score)>();

            // 2) Fetch full product details for the ranked products
            // This ensures we have all necessary information for context
            // when generating the answer
            foreach (var r in ranked)
            { 
                var fullProduct = await _productRepo.GetByIdAsync(r.Product.Id);
                products.Add((fullProduct ?? r.Product, r.Score));
            }

            // 3) Build context from the retrieved products
            // This context will be provided to the AI model
            // to help generate a relevant answer
            string context = BuildContextFromProducts(products);

            // 4) Build user prompt combining the question and the context
            // This prompt guides the AI model in generating the answer
            string userPrompt = BuildUserPrompt(question, context);

            // 5) Define system prompt with instructions for the AI model
            // This sets the behavior and constraints for the generated answer
            // For example, instructing the model to only use provided context
            // and not to invent information
            string systemPrompt =
                "You are a helpful e-commerce assistant. " +
                "Use ONLY the provided product context to answer the user's question. " +
                "If you are not sure, say you don't know. " +
                "Do not invent products that are not in the context.";

            // 6) Get the AI-generated answer using the chat service
            // This involves sending the system and user prompts to the AI model
            // and receiving the generated response
            string answer = await _aIChatService.GetChatCompletionAsync(systemPrompt, userPrompt);

            // 7) Prepare the response view model
            // This includes the generated answer and details of the relevant products
            // to be returned to the user
            var productVms = products.Select(p => new ChatProductResultVM
            {
                ProductId = p.Product.Id,
                Title = p.Product.Title ?? string.Empty,
                BrandName = p.Product.Brand?.Name,
                CategoryName = p.Product.Category?.Name,
                Score = p.Score
            }).ToList();

            // 8) Return the final response result
            // Indicates success and includes the answer and product details
            var responseVm = new ChatResponseVM
            {
                Answer = answer,
                Products = productVms
            };

            // 9) Return the response wrapped in a ResponseResult
            return new ResponseResult<ChatResponseVM>(
                responseVm,
                "Answer generated successfully.",
                true
            );
        }

        // Helper method to build context string from a list of products
        // This context includes key details of each product
        // to be used by the AI model for generating answers 
        private string BuildContextFromProducts(List<(Product Product, double Score)> products)
        {
            // Using StringBuilder for efficient string concatenation
            // and formatting of the context string 
            var sb = new StringBuilder();

            // Adding a header to indicate the start of product context
            sb.AppendLine("Here are some relevant products from the catalog:");
            // Adding a blank line for better readability
            sb.AppendLine();

            // Iterating through each product and appending its details to the context
            // This includes ID, title, brand, category, description, price, rating, and relevance score
            // Each product is separated by a line of dashes for clarity
            foreach (var (p, score) in products)
            {
                sb.AppendLine($"Product ID: {p.Id}");
                sb.AppendLine($"Title: {p.Title}");
                if (p.Brand != null)
                    sb.AppendLine($"Brand: {p.Brand.Name}");
                if (p.Category != null)
                    sb.AppendLine($"Category: {p.Category.Name}");
                sb.AppendLine($"Description: {p.Description}");
                sb.AppendLine($"Price: {p.Price}");
                sb.AppendLine($"Rating: {p.Rating}");
                sb.AppendLine($"RelevanceScore: {score:F3}");
                sb.AppendLine(new string('-', 40));
            }

            return sb.ToString();
        }

        // Helper method to build the user prompt for the AI model
        // Combines the user's question with the context of relevant products
        // and specifies the task for the AI to perform
        private string BuildUserPrompt(string question, string context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("CONTEXT:");
            sb.AppendLine(context);
            sb.AppendLine();
            sb.AppendLine("USER QUESTION:");
            sb.AppendLine(question);
            sb.AppendLine();
            sb.AppendLine("TASK:");
            sb.AppendLine("Based on the CONTEXT above, answer the USER QUESTION. " +
                          "Recommend one or more products if possible and explain why. " +
                          "Do not mention products that are not in the context.");
            return sb.ToString();
        }
    }
}
