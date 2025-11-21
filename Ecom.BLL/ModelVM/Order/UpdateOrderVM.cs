using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.DAL.Enum;

namespace Ecom.BLL.ModelVM.Order
{
    public class UpdateOrderVM
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? ShippingAddress { get; set; }
    }
}
