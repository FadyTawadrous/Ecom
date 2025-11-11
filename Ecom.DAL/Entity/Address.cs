namespace Ecom.DAL.Entity
{
    public class Address
    {
        [Key]
        public int Id { get; private set; }
        public string? Street { get; private set; } = string.Empty;
        public string? City { get; private set; } = string.Empty;
        public string? Country { get; private set; } = string.Empty;
        public string? PostalCode { get; private set; } = string.Empty;
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }        
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foriegn Keys
        [ForeignKey("AppUser")]
        public string? AppUserId { get; private set; } = string.Empty;

        // Navigation Properties
        public virtual AppUser? AppUser { get; private set; }

        // Logic
        public Address() { }
        public Address(string street, string city, string country, string postalCode, string createdBy,
            string appUserId)
        {
            Street = street;
            City = city;
            Country = country;
            PostalCode = postalCode;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
            AppUserId = appUserId;
        }

        public bool Update(string street, string city, string country, string postalCode, string updatedBy)
        {
            if (!string.IsNullOrEmpty(updatedBy))
            {
                Street = street;
                City = city;
                Country = country;
                PostalCode = postalCode;                
                UpdatedBy = updatedBy;
                UpdatedOn = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public bool ToggleDelete(string deletedBy)
        {
            if (!string.IsNullOrEmpty(deletedBy))
            {
                IsDeleted = !IsDeleted;
                DeletedBy = deletedBy;
                DeletedOn = DateTime.UtcNow;                
                return true;
            }
            return false;
        }
    }
}
