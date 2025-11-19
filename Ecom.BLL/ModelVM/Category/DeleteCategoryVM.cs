
namespace Ecom.BLL.ModelVM.Category
{
    public class DeleteCategoryVM
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Url]
        public string ImageUrl { get; set; }
        public string? DeletedBy { get; set; }
    }
}
