
namespace Ecom.BLL.ModelVM.Category
{
    public class DeleteCategoryVM
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string? DeletedBy { get; set; }
    }
}
