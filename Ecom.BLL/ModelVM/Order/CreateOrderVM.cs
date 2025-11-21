using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.BLL.ModelVM.OrderItem;

namespace Ecom.BLL.ModelVM.Order
{
    public class CreateOrderVM
    {
        public string AppUserId { get; set; } = null!;
        public DateTime? DeliveryDate { get; set; }
        public string ShippingAddress { get; set; } = null!;

        // Items passed when creating an order
        public List<CreateOrderItemVM> Items { get; set; } = new();
    }
}
