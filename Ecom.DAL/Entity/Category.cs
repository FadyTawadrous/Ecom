
namespace Ecom.DAL.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; private set; }
        [Required, MaxLength(50)]
        public string Name { get; private set; } = string.Empty;
        public string? ImageUrl { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public virtual ICollection<Product>? Products { get; private set; }

        // Logic
        public Category() { }
        public Category(string name, string imageUrl, string createdBy)
        {
            Name = name;
            ImageUrl = imageUrl;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }

        public bool Update(string name, string imageUrl, string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                Name = name;
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
