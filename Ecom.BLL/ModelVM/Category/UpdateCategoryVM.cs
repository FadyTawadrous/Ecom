
namespace Ecom.BLL.ModelVM.Category
{
    public class UpdateCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
