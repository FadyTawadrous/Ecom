using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.BLL.ModelVM.OrderItem;
using Ecom.DAL.Enum;

namespace Ecom.BLL.ModelVM.Order
{
    public class GetOrderVM
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;

        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string? TrackingNumber { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string AppUserId { get; set; } = null!;
        public string CustomerName { get; set; } = null!; // from User table

        public List<GetOrderItemVM> Items { get; set; } = new();

    }
}
