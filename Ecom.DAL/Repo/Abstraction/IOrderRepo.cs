
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IOrderRepo
    {
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>>? filter = null);
        Task<IEnumerable<Order>> GetByUserIdAsync(string appUserId);
        Task<Order?> GetWithItemsAsync(int id);

        Task AddAsync(Order order);
        Task UpdateAsync(int id, string UpdatedBy,OrderStatus orderStatus);
        Task DeleteAsync(int Id, string deletedBy);

        Task<int> SaveChangesAsync();


    }
}
