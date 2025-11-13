using System;
using System.Collections.Generic;
namespace Ecom.BLL.ModelVM.WishlistItem
{
    public class GetWishlistItemVM
    {
        public int Id { get; set; }

        // Product-Related data
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
