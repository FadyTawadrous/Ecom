
namespace Ecom.BLL.ModelVM.CartItem
{
    public class DeleteCartItemVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
