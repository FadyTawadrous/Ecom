
namespace Ecom.BLL.ModelVM.Category
{
    public class AddCategoryVM
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Url]
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? CreatedBy { get; set; }

    }
}
