
using Ecom.BLL.Responses;

namespace Ecom.DAL.Repo.Abstraction
{
    public interface IAddressRepo
    {
        // Query Methods
        Task<Address?> GetByIdAsync(int id,
            params Expression<Func<Address, object>>[] includes);
        Task<PaginatedResult<Address>> GetAllByUserIdAsync(string userId,
            Expression<Func<Address, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<Address, object>>[] includes);
        Task<PaginatedResult<Address>> GetAllAsync(
            Expression<Func<Address, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<Address, object>>[] includes);

        // Command Methods
        Task<bool> AddAsync(Address newAddress);
        Task<bool> UpdateAsync(Address newAddress);
        Task<bool> ToggleDeleteStatusAsync(int id, string userModified);
        Task<bool> DeleteAsync(int id);
    }
}
