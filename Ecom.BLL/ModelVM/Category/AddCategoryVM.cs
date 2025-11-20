
namespace Ecom.BLL.ModelVM.Category
{
    public class AddCategoryVM
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? CreatedBy { get; set; }
    }
}
