

namespace Ecom.BLL.ModelVM.ProductImageURL
{
    public class GetProductImageUrlVM
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }  // optional: from navigation
        public string? CreatedBy { get; set; }
      
    }
}
