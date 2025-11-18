
namespace Ecom.DAL.Entity
{
    public class Cart
    {
        public int Id { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public string? DeletedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Computed properies (Not mapped to the database)
        public decimal TotalAmount => CartItems?.Where(i => !i.IsDeleted)
            .Sum(i => i.TotalPrice) ?? 0;

        // Foriegn Keys
        public string AppUserId { get; private set; } = null!;

        // Navigation Properties
        public virtual AppUser AppUser { get; private set; } = null!;
        public virtual ICollection<CartItem>? CartItems { get; private set; }

        // Logic
        public Cart() { }
        public Cart(string appUserId, string createdBy)
        {
            AppUserId = appUserId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }

        public bool Update(string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                UpdatedOn = DateTime.UtcNow;
                UpdatedBy = userModified;
                return true;
            }
            return false;
        }
        public bool ToggleDelete(string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                IsDeleted = !IsDeleted;
                UpdatedBy = userModified;
                UpdatedOn = DateTime.UtcNow;
                return true;
            }
            return false;
        }

    }
}
