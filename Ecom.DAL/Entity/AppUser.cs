
namespace Ecom.DAL.Entity
{
    public class AppUser : IdentityUser
    {
        // IdentityUser provide: Email, Password, PhoneNumber, Role management
        public string? DisplayName { get; private set; }
        public string? ProfileImageUrl { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public virtual Cart? Cart { get; private set; }
        public virtual ICollection<Order>? Orders { get; private set; }
        public virtual ICollection<Address>? Addresses { get; private set; }
        public virtual ICollection<ProductReview>? ProductReviews { get; private set; }
        public virtual ICollection<WishlistItem>? WishlistItems { get; private set; }
        public virtual ICollection<FaceId>? FaceIds { get; private set; }

        // Logic
        public AppUser() { }
        public AppUser(string email, string? displayName, string? profileImageUrl, string createdBy,
            string? phoneNumber)
        {
            Email = email;
            UserName = email;
            DisplayName = displayName;
            PhoneNumber = phoneNumber;
            CreatedBy = createdBy;
            ProfileImageUrl = profileImageUrl;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }

        public bool Update(string? displayName, string? profileImageUrl,string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                DisplayName = displayName;
                ProfileImageUrl = profileImageUrl;
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
