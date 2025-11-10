

namespace Ecom.BLL.ModelVM.ProductImageURL
{
    public class CreateProductImageUrlVM
    {
        public string? ImageUrl { get; set; } // Will be set after upload
        public IFormFile? Image { get; set; } // File input from user
        public int ProductId { get; set; }
        public string? CreatedBy { get; set; } // username or admin name
    }
}
