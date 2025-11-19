
namespace Ecom.BLL.ModelVM.CartItem
{
    public class AddCartItemVM
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        public string CreatedBy { get; set; }
        
    }
}
