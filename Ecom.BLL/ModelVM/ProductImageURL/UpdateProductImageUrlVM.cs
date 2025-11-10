

namespace Ecom.BLL.ModelVM.ProductImageURL
{
    public class UpdateProductImageUrlVM
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public int ProductId { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
