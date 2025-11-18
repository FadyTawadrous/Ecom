
namespace Ecom.BLL.ModelVM.CartItem
{
    public class GetCartItemVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int CartId { get; set; }
        public string CartName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

    }
}
