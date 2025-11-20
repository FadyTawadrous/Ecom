using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.ProductReview
{
    
    public class ProductReviewGetVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Rating { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        // Relation keys
        public int ProductId { get; set; }
        public string AppUserId { get; set; } = null!;

        // Extra read-only fields for display
        public string? ProductTitle { get; set; }
        public string? AppUserDisplayName { get; set; }
    }

}
