
using Ecom.DAL.Enum;

namespace Ecom.BLL.ModelVM.Payment
{
    public class GetPaymentVM
    {
        public int Id { get; set; }
        public PaymentMethod? PaymentMethod { get; set; } // Enum: Cash, Card, Paypal
        public PaymentStatus Status { get; set; } // Enum: Pending, Completed, Failed
        public decimal TotalAmount { get; set; }
        public string? TransactionId { get; set; }
        public int OrderId { get; set; }
    }
}
