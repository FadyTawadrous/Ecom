
using Ecom.DAL.Enum;

namespace Ecom.BLL.ModelVM.Payment
{
    public class CreatePaymentVM
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }

        public string? CreatedBy { get; set; }
    }
}
