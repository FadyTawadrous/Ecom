
namespace Ecom.BLL.ModelVM.Cart
{
    public class GetCartVM
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; }

        // Foriegn Keys
        public string AppUserId { get; set; }

        // Navigation Properties
        public virtual ICollection<GetCartItemVM>? CartItems { get; set; }

        // Audit Fields for dashboard display
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
