
using Microsoft.EntityFrameworkCore;

namespace Ecom.DAL.Repo.Implementation
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _db;

        public ProductRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ Get All
        public async Task<IEnumerable<Product>> GetAllAsync(
            Expression<Func<Product, bool>>? filter = null,
            params Expression<Func<Product, object>>[] includes)
        {
            IQueryable<Product> query = _db.Products;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        // ✅ Get By ID
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImageUrls)
                .Include(p => p.ProductReviews)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // ✅ Add
        public async Task<bool> AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            return await _db.SaveChangesAsync() > 0;
        }

        // ✅ Update
        public async Task<bool> UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            return await _db.SaveChangesAsync() > 0;
        }

        // ✅ Toggle Delete
        public async Task<bool> ToggleDeleteStatusAsync(int id, string userModified)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return false;

            product.ToggleDelete(userModified);
            return await _db.SaveChangesAsync() > 0;
        }


        //PRODUCT REPOSITORY — Decrease Stock
        public async Task<bool> DecreaseStockAsync(int productId, int quantity)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null) return false;

            bool removed = product.TryRemoveStock(quantity);
            if (!removed) return false;

            return await _db.SaveChangesAsync() > 0;
        }

        
        public async Task<bool> IncreaseStockAsync(int productId, int quantity)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null) return false;
            
            product.AddStock(quantity);

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRatingAsync(int productId, decimal newAverageRating)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null)
                return false;

            product.UpdateRating(newAverageRating);  // <-- USE ENTITY FUNCTION

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddToQuantitySoldAsync(int productId, int quantity)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null) return false;

            product.AddToQuantitySold(quantity);

            return await _db.SaveChangesAsync() > 0;
        }
    }



}
