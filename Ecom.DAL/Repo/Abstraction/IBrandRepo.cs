using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DAL.Repo.Abstraction
{
    public interface IBrandRepo
    {
        Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? filter = null);
        Task<Brand?> GetByIdAsync(int id);
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(int id, string deletedBy);
        Task<int> SaveChangesAsync();
    }
}
