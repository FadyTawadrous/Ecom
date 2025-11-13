namespace Ecom.BLL.ModelVM.WishlistItem
{
    public class CreateWishlistItemVM
    {
        public string AppUserId { get; private set; } = null!;
        public int ProductId { get; private set; }
        public string CreatedBy { get; private set; } = null!;
    }
}
