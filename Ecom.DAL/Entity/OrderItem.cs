
namespace Ecom.DAL.Entity
{
    public class OrderItem
    {
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        // Snapshot the title, so if the original Product.Title changes,
        // this order's history is not affected.
        public string ProductTitle { get; private set; } = null!;
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foreign Keys
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }

        // Navigation Properties
        public virtual Order Order { get; private set; } = null! ;
        public virtual Product Product { get; private set; } = null!;

        // Logic
        public OrderItem() { }

        public OrderItem(int productId, int orderId, int quantity, decimal unitPrice, string createdBy
            , string productTitle)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CreatedOn = DateTime.UtcNow;
            CreatedBy = createdBy;
            IsDeleted = false;
            TotalPrice = UnitPrice * Quantity;
            ProductTitle = productTitle;
        }

        // Order Item must be immutable (no update, no delete)

    }
}
