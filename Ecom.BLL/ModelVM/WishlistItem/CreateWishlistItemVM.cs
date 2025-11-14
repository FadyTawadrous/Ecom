namespace Ecom.BLL.ModelVM.WishlistItem
{
    public class CreateWishlistItemVM
    {
        public string AppUserId { get; set; } = null!;
        public int ProductId { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
