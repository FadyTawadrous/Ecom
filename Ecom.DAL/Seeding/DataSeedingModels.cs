
namespace Ecom.DAL.Seeding
{
    public static class DataSeedingModels
    {
        public class DummyResponse
        {
            public List<DummyProduct> Products { get; set; } = new();
        }

        public class DummyProduct
        {
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public decimal Price { get; set; }
            public decimal DiscountPercentage { get; set; }
            public decimal Rating { get; set; }
            public int Stock { get; set; }
            public string Brand { get; set; } = ""; // String in JSON
            public string Category { get; set; } = ""; // String in JSON
            public string Thumbnail { get; set; } = "";
            public List<string> Images { get; set; } = new();
            public List<DummyReview> Reviews { get; set; } = new();
        }

        public class DummyReview
        {
            public int Rating { get; set; }
            public string Comment { get; set; } = "";
            public string ReviewerName { get; set; } = "";
            public string ReviewerEmail { get; set; } = "";
        }
    }
}
