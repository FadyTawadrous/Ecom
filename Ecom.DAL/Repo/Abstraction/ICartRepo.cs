
namespace Ecom.DAL.Repo.Abstraction
{
    public interface ICartRepo
    {
        Task<Cart> GetByUserIdAsync(string userId, 
                                    params Expression<Func<Cart, object>>[] includes);
        Task<Cart> GetByIdAsync(int id,
                                    params Expression<Func<Cart, object>>[] includes);
        Task<bool> AddAsync(Cart newCart);

        Task<bool> ClearCartAsync(int cartId);
    }
}
