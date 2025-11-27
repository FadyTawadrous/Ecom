using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.OrderItem
{
    public class GetOrderItemVM
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = null!; // join from product table
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
