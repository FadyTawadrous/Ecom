
namespace Ecom.BLL.ModelVM.CartItem
{
    public class UpdateCartItemVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        public string? UpdatedBy { get; private set; }
    }
}
