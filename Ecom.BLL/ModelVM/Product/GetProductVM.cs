

namespace Ecom.BLL.ModelVM.Product
{
    public class GetProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Rating { get; set; }
        public int Stock { get; set; }
        public int QuantitySold { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsDeleted { get; set; }
    }

}
