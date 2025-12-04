using Ecom.BLL.Service.Abstraction.Chatbot;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;

namespace Ecom.BLL.Service.Implementation.Chatbot
{
    public class EmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public EmbeddingService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<double[]> GenerateEmbeddingAsync(string inputText)
        {
            // 1) Read config
            var apiKey = _config["Embeddings:ApiKey"];
            var model = _config["Embeddings:Model"]
                           ?? "nomic-ai/nomic-embed-text-v1.5";
            var endpoint = _config["Embeddings:Endpoint"]
                           ?? "https://api.fireworks.ai/inference/v1/embeddings";

            // 2) Build Fireworks-compatible request body
            var requestBody = new
            {
                model = model,
                input = new[] { inputText }   // Fireworks expects an array
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 3) Set headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // 4) Call Fireworks
            using var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Embedding API error {response.StatusCode}: {errBody}");
            }

            // 5) Parse response -> double[]
            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            // Fireworks returns: { "data": [ { "embedding": [ ... ] } ], ... }
            var vector = doc.RootElement
                            .GetProperty("data")[0]
                            .GetProperty("embedding")
                            .EnumerateArray()
                            .Select(e => e.GetDouble())
                            .ToArray();

            return vector;
        }
    }
}
