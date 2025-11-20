
namespace Ecom.DAL.Repo.Abstraction
{
    public interface ICartItemRepo
    {
        Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId,
            params Expression<Func<CartItem, object>>[] includes);

        Task<CartItem> GetByIdAsync(int id,
            params Expression<Func<CartItem, object>>[] includes);
        Task<CartItem> GetByCartIdAndProductIDAsync(int cartId, int productId);
        Task<bool> AddAsync(CartItem newCartItem);
        Task<bool> UpdateAsync(CartItem newCartItem);
        Task<bool> DeleteAsync(int id);
    }
}
