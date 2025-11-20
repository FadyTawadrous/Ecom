
using System.Runtime.Intrinsics.X86;

namespace Ecom.DAL.Entity
{
    public class Product
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal Price { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public decimal Rating { get; private set; } // Controlled by UpdateRating()
        public int Stock { get; private set; } // Controlled by TryRemoveStock()
        public int QuantitySold { get; private set; } // Controlled by AddToQuantitySold()
        public string? ThumbnailUrl { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foriegn Keys
        public int BrandId { get; private set; }
        public int CategoryId { get; private set; }

        // Navigation Properties
        public virtual Brand Brand { get; private set; } = null!;
        public virtual Category Category { get; private set; } = null!;
        public virtual ICollection<WishlistItem>? WishlistItems { get; private set; }
        public virtual ICollection<ProductImageUrl>? ProductImageUrls { get; private set; }
        public virtual ICollection<ProductReview>? ProductReviews { get; private set; }
        

        // Logic
        public Product() { }
        public Product(string title, string description, decimal price, decimal discountPercentage, int stock,
            string thumbnailUrl, string createdBy, int brandId, int categoryId)
        {
            Title = title;
            Description = description;
            Price = price;
            DiscountPercentage = discountPercentage;
            Stock = stock;
            ThumbnailUrl = thumbnailUrl;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
            BrandId = brandId;
            CategoryId = categoryId;
            QuantitySold = 0;
        }

        public bool Update(string title, string description, decimal price, decimal discountPercentage,
            string thumbnailUrl, int stock, string userModified, int brandId, int categoryId)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                Title = title;
                Description = description;
                Price = price;
                DiscountPercentage = discountPercentage;
                ThumbnailUrl = thumbnailUrl;
                Stock = stock;
                BrandId = brandId;
                CategoryId = categoryId;
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

        //How to use this (in your BLL): When your ReviewService adds a new ProductReview, it will
        //    (1) save the review, 
        //    (2) recalculate the average rating for the product, 
        //    (3) call product.UpdateRating(newAverage)
        //    (4) save the product
        public void UpdateRating(decimal newAverageRating)
        {
            if (newAverageRating >= 0 && newAverageRating <= 5)
            {
                Rating = newAverageRating;
            }
        }



        // This method tries to remove stock and returns true/false on success.
        // This prevents overselling.

        // How to use this (in your BLL): When your OrderService processes an order, it will loop through the
        // orderitems. For each item, it will:
        // if (!product.TryRemoveStock(item.Quantity)) { throw new Exception("Not enough stock!");}
        // product.AddToQuantitySold(item.Quantity);
        // _context.SaveChanges();
        public bool TryRemoveStock(int quantityToDecrease)
        {
            if (quantityToDecrease <= 0) return false; // Can't remove 0 or less
            if (Stock < quantityToDecrease)
            {
                return false; // Not enough stock!
            }

            Stock -= quantityToDecrease;
            return true;
        }

        public void AddStock(int quantityToIncrease)
        {
            if (quantityToIncrease <= 0) return;

            Stock += quantityToIncrease;
        }

        // This method adds to the sold quantity.
        public void AddToQuantitySold(int quantitySold)
        {
            if (quantitySold > 0)
            {
                QuantitySold += quantitySold;
            }
        }
    }
}
