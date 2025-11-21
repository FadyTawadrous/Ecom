using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.DAL.Enum;

namespace Ecom.BLL.ModelVM.OrderStatusVM
{
    public class UpdateOrderStatusVM
    {
        public OrderStatus NewStatus { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public string? TrackingNumber { get; set; }
    }
}
