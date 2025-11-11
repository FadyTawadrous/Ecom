
namespace Ecom.DAL.Entity
{
    public class ProductImageUrl
    {
        public int Id { get; private set; }
        public string ImageUrl { get; private set; } = null!;
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foriegn Keys
        public int ProductId { get; private set; }

        // Navigation Properties
        public Product Product { get; private set; } = null!;

        // Logic
        public ProductImageUrl() { }
        public ProductImageUrl(string imageUrl, int productId, string createdBy)
        {
            ImageUrl = imageUrl;
            ProductId = productId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }

        public bool Update(string imageUrl, string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                ImageUrl = imageUrl;
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
