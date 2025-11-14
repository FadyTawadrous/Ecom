using System;
using System.Collections.Generic;
using System.Linq;
namespace Ecom.BLL.ModelVM.WishlistItem
{
    public class DeleteWishlistItemVM
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; } = null!;
    }
}
