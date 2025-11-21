
namespace Ecom.DAL.Entity
{
    public class Order
    {
        public int Id { get; private set; }
        public DateTime? DeliveryDate { get; private set; }
        public OrderStatus Status { get; private set; } // Enum
        public decimal TotalAmount { get; private set; } // Set by RecalculateTotal()
        public string ShippingAddress { get; private set; } = null!;
        public string? TrackingNumber { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }
        public string OrderNumber { get; private set; }

        // Foriegn Keys
        public string AppUserId { get; private set; } = null!;

        // Navigation Properties
        public virtual AppUser AppUser { get; private set; } = null!;
        public virtual Payment? Payment { get; private set; }
        public virtual ICollection<OrderItem> OrderItems { get; private set; }

        // Logic
        public Order() 
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(string appUserId, DateTime deliveryDate, string shippingAddress,string createdBy ,List<OrderItem> Items)
        {
            AppUserId = appUserId;
            Status = OrderStatus.Pending;
            ShippingAddress = shippingAddress;
            DeliveryDate = deliveryDate;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
            TotalAmount = 0;
            OrderItems = Items;
            OrderNumber = $"ORD-{Guid.NewGuid().ToString()[..8].ToUpper()}"; 
        }

        public bool Update(OrderStatus orderStatus, string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                Status = orderStatus;
                UpdatedBy = userModified;
                UpdatedOn = DateTime.UtcNow;
                if (string.IsNullOrEmpty(TrackingNumber) && orderStatus == OrderStatus.Shipped)
                    TrackingNumber = $"TRK-{Guid.NewGuid().ToString()[..8].ToUpper()}";
                return true;
            }
            return false;
        }

        public bool ToggleDelete(string userModified)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                IsDeleted = !IsDeleted;
                UpdatedBy = userModified;
                UpdatedOn = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public void RecalculateTotal()
        {
            if (OrderItems != null && OrderItems.Count > 0)
            {
                TotalAmount = OrderItems
                    .Where(i => !i.IsDeleted)
                    .Sum(i => i.TotalPrice);
            }
            else
            {
                TotalAmount = 0;
            }
        }

        public void AddItem(OrderItem item)
        {
            OrderItems.Add(item);
            RecalculateTotal();
        }
        public void RemoveItem(OrderItem item)
        {
            OrderItems.Remove(item); // needs to be tested 
            RecalculateTotal();
        }

        public bool IsValidStatusTransition(OrderStatus current , OrderStatus next)
        {
            return (current, next) switch
            {
                //  Normal Order Flow
                (OrderStatus.Pending, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,


                //customer Adming Cancels Before Shipping 
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Cancelled) => true,

                //and order Can be returned After Delivered
                (OrderStatus.Delivered, OrderStatus.Returned) => true,

                // Cannot Change after Cancelled 
                (OrderStatus.Cancelled, _) => true,
                // Cannot Change After Returned 
                (OrderStatus.Returned, _) => false,

                // Delivered cannot move backward except Returned
                (OrderStatus.Delivered, OrderStatus.Shipped) => false,
                (OrderStatus.Delivered, OrderStatus.Processing) => false,
                (OrderStatus.Delivered, OrderStatus.Pending) => false,

                //Default : Don't Allow any Other Changes 
                _ => false
            };
        }

        public bool ChangeStatus(OrderStatus newStatus, string userUpdator)
        {
            if (!IsValidStatusTransition(Status,newStatus))
            {
                return false;
            }
            Status = newStatus;
            UpdatedBy = userUpdator;
            UpdatedOn = DateTime.Now;
            return true;
        }

    }
}
