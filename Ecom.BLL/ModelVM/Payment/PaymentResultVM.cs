
using Ecom.DAL.Enum;

namespace Ecom.BLL.ModelVM.Payment
{
    public class PaymentResultVM
    {
        [Required]
        public int PaymentId { get; set; }

        public string? TransactionId { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

    }
}
