
namespace Ecom.DAL.Entity
{
    public class ProductReview
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal Rating { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foriegn Keys
        public int ProductId { get; private set; }
        public string AppUserId { get; private set; } = null!;

        // Navigation Properties
        public virtual Product Product { get; private set; } = null!;
        public virtual AppUser AppUser { get; private set; } = null!;

        // Logic
        public ProductReview() { }
        public ProductReview(string title, string description, decimal rating, string createdBy, int productId,
            string appUserId)
        {
            Title = title;
            Description = description;
            Rating = rating;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
            ProductId = productId;
            AppUserId = appUserId;
        }

        public bool Update(string title, string description, decimal rating, string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                Title = title;
                Description = description;
                Rating = rating;
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
                DeletedOn = DateTime.UtcNow;
                DeletedBy = userModified;
                return true;
            }
            return false;
        }

    }
}
