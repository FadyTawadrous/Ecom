
namespace Ecom.BLL.ModelVM.Product
{
    public class CreateProductVM
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int Stock { get; set; }
        public IFormFile? Thumbnail { get; set; }      // uploaded file
        public string? ThumbnailUrl { get; set; }     // will be set after upload
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string? CreatedBy { get; set; }
    }
}
