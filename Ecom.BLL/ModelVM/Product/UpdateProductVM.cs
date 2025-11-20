
namespace Ecom.BLL.ModelVM.Product
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int Stock { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? ThumbnailUrl { get; set; } // after upload
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string? UpdatedBy { get; set; }
    }

}
