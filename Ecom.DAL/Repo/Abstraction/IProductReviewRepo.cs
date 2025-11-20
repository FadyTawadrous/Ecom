using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DAL.Repo.Abstraction
{
    public interface IProductReviewRepo
    {
        // Queries
        Task<IEnumerable<ProductReview>> GetAllAsync(
            Expression<Func<ProductReview, bool>>? filter = null,
            params Expression<Func<ProductReview, object>>[] includes);

        Task<ProductReview?> GetByIdAsync(int id);

        // Commands
        Task<bool> AddAsync(ProductReview review);
        Task<bool> UpdateAsync(ProductReview review);
        Task<bool> ToggleDeleteStatusAsync(int id, string userModified);
    }
}
