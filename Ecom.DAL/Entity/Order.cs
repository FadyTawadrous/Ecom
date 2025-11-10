
namespace Ecom.DAL.Entity
{
    public class Order
    {
        public int Id { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public OrderStatus Status { get; private set; } // Enum
        public decimal TotalAmount { get; private set; } // Set by RecalculateTotal()
        public string ShippingAddress { get; private set; } = null!;
        public string? TrackingNumber { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foriegn Keys
        public string AppUserId { get; private set; } = null!;

        // Navigation Properties
        public virtual AppUser AppUser { get; private set; } = null!;
        public virtual Payment? Payment { get; private set; }
        public virtual ICollection<OrderItem>? OrderItems { get; private set; }

        // Logic
        public Order() 
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(string appUserId, DateTime deliveryDate, string shippingAddress,string createdBy)
        {
            AppUserId = appUserId;
            Status = OrderStatus.Pending;
            ShippingAddress = shippingAddress;
            DeliveryDate = deliveryDate;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
            TotalAmount = 0;
            OrderItems = new List<OrderItem>();
        }

        public bool Update(OrderStatus orderStatus, string userModified, string? trackingNumber)
        {
            if (!string.IsNullOrEmpty(userModified))
            {
                Status = orderStatus;
                UpdatedBy = userModified;
                UpdatedOn = DateTime.UtcNow;
                TrackingNumber = trackingNumber;
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

    }
}
