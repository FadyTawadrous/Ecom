
using Ecom.DAL.Entity;

namespace Ecom.BLL.ModelVM.Category
{
    public class GetCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        // Audit Fields for dashboard display
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation Property - List of Products in this Category
        public ICollection<ProductInCategoryVM>? Products { get; set; }
    }

    public class ProductInCategoryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Rating { get; set; } // Controlled by UpdateRating()
        public int Stock { get; set; } // Controlled by TryRemoveStock()
        public int QuantitySold { get; set; } // Controlled by AddToQuantitySold()
        public string? ThumbnailUrl { get; set; }
    }
}
