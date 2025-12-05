using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.ProductReview
{
    
    public class ProductReviewCreateVM
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Rating { get; set; }
        public int ProductId { get; set; }
        public string ?AppUserId { get; set; } = null!;
        public string ?CreatedBy { get; set; } = "system";
    }

}
