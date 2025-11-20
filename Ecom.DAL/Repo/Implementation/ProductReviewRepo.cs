using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DAL.Repo.Implementation
{
    public class ProductReviewRepo : IProductReviewRepo
    {
        private readonly ApplicationDbContext _db;

        public ProductReviewRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ Get All
        public async Task<IEnumerable<ProductReview>> GetAllAsync(
            Expression<Func<ProductReview, bool>>? filter = null,
            params Expression<Func<ProductReview, object>>[] includes)
        {
            IQueryable<ProductReview> query = _db.ProductReviews;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        // ✅ Get by ID
        public async Task<ProductReview?> GetByIdAsync(int id)
        {
            return await _db.ProductReviews
                .Include(r => r.Product)
                .Include(r => r.AppUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

       

        // ✅ Add
        public async Task<bool> AddAsync(ProductReview review)
        {
            await _db.ProductReviews.AddAsync(review);
            return await _db.SaveChangesAsync() > 0;
        }

        // ✅ Update
        public async Task<bool> UpdateAsync(ProductReview review)
        {
            _db.ProductReviews.Update(review);
            return await _db.SaveChangesAsync() > 0;
        }

        // ✅ Toggle Delete
        public async Task<bool> ToggleDeleteStatusAsync(int id, string userModified)
        {
            var review = await _db.ProductReviews.FindAsync(id);
            if (review == null) return false;

            review.ToggleDelete(userModified);
            return await _db.SaveChangesAsync() > 0;
        }
    }

}
