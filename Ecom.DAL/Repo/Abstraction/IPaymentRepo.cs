
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IPaymentRepo
    {
        // Query Methods
        Task<IEnumerable<Payment>> GetAllAsync(
            Expression<Func<Payment, bool>>? Filter = null,
            params Expression<Func<Payment, object>>[] includes);
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByOrderIdAsync(int orderId); 

        // Command Methods
        Task<bool> AddAsync(Payment payment);
        Task<bool> UpdateAsync(Payment payment);
        Task<bool> ToggleDeleteStatusAsync(int id, string userModified);
    }
}
