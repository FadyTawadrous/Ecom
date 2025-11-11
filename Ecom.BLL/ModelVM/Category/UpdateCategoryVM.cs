
namespace Ecom.BLL.ModelVM.Category
{
    public class UpdateCategoryVM
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Url]
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? UpdatedBy { get; private set; }
    }
}
