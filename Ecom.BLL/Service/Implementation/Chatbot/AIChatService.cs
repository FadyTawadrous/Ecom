using Ecom.BLL.Service.Abstraction.Chatbot;
using System.Text.Json;

namespace Ecom.BLL.Service.Implementation.Chatbot
{
    // Service to interact with LLM API for chat completions
    // ( LLM ) is a Large Language Model like GPT-4, etc. 
    public class AIChatService : IAIChatService
    {
        // HttpClient to make API requests
        private readonly HttpClient _httpClient;
        // Configuration to access LLM settings
        private readonly IConfiguration _config;

        public AIChatService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // Get chat completion from LLM API
        // systemPrompt: Instructions for the LLM
        // userPrompt: User's input prompt
        // Returns the LLM's response as a string
        public async Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt)
        {
            try
            {
                // 1) Prepare request to LLM API
                // Get API settings from configuration
                // These settings should be defined in appsettings.json or environment variables
                var apiKey = _config["LLM:ApiKey"];
                var endpoint = _config["LLM:Endpoint"];
                var model = _config["LLM:Model"];

                // 2) Build request body
                // Using the chat completion format
                // Refer to the LLM API documentation for details
                var body = new
                {
                    model,
                    messages = new[]
                    {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                    temperature = 0.2,
                    max_tokens = 512
                };

                // 3) Serialize body to JSON and prepare HTTP content for POST request 
                var json = JsonSerializer.Serialize(body);

                // Set up HTTP content with appropriate headers
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 4) Make POST request to LLM API 
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // 5) Send request and handle response 
                using var response = await _httpClient.PostAsync(endpoint, content);

                // Check for successful response
                // If not successful, read error message and throw exception
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"LLM API error: {response.StatusCode} - {errorBody}");
                }

                // 6) Parse response JSON to extract the generated content
                // The response structure may vary based on the LLM API used
                var responseJson = await response.Content.ReadAsStringAsync();

                // Parse JSON to get the content from the first choice
                using var doc = JsonDocument.Parse(responseJson);

                // Extract the content text from the response
                // Navigate through the JSON structure to get the desired field
                var contentText = doc.RootElement
                                     .GetProperty("choices")[0]
                                     .GetProperty("message")
                                     .GetProperty("content")
                                     .GetString();

                // 7) Return the generated content which is the LLM's response 
                return contentText ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Log exception (logging not implemented here)
                throw new Exception("Error while getting chat completion from LLM", ex);
            }
        }
    }
}
