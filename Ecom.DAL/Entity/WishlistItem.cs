
namespace Ecom.DAL.Entity
{
    public class WishlistItem
    {
        public int Id { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }      
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public bool IsDeleted { get; private set; }

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
            IsDeleted = false;
        }

        public bool ToggleDelete(string deletedBy)
        {
            if (!string.IsNullOrEmpty(deletedBy))
            {
                IsDeleted = !IsDeleted;
                DeletedOn = DateTime.UtcNow;
                DeletedBy = deletedBy;
                return true;
            }
            return false;
        }
    }
}
