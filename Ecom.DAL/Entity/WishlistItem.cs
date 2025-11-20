namespace Ecom.DAL.Entity
{
    public class WishlistItem
    {
        public int Id { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }

        // Foriegn Keys
        public string AppUserId { get; private set; } = null!;
        public int ProductId { get; private set; }

        // Navigation Properties
        public virtual AppUser AppUser { get; private set; } = null!;
        public virtual Product Product { get; private set; } = null!;

        // Logic
        public WishlistItem() { }
        public WishlistItem(string appUserId, int productId, string createdBy)
        {
            AppUserId = appUserId;
            ProductId = productId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }
    }
}
