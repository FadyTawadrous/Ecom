
namespace Ecom.DAL.Repository.Implementation
{
    public class ProductImageUrlRepo : IProductImageUrlRepo
    {
        private readonly ApplicationDbContext _db;

        public ProductImageUrlRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<ProductImageUrl>> GetAllAsync(Expression<Func<ProductImageUrl, bool>>? filter = null,
            params Expression<Func<ProductImageUrl, object>>[] includes)
        {
            try
            {
                IQueryable<ProductImageUrl> query = _db.ProductImageUrls.AsQueryable();

                if (filter != null)
                    query = query.Where(filter);

                foreach (var include in includes)
                    query = query.Include(include);

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductImageUrl?> GetByIdAsync(int id)
        {
            try
            {
                var img = await _db.ProductImageUrls
                    .Include(i => i.Product)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (img != null)
                    return img;

                throw new KeyNotFoundException($"ProductImageUrl with Id {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddAsync(ProductImageUrl newImage)
        {
            try
            {
                if (newImage == null)
                    return false;

                var result = await _db.ProductImageUrls.AddAsync(newImage);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;

               // After saving, EF automatically fills Id(primary key).
               //If it’s greater than 0 → insertion succeeded.
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ProductImageUrl newImage)
        {
            try
            {
                if (newImage == null)
                    return false;

                var oldImage = await _db.ProductImageUrls.FindAsync(newImage.Id);
                if (oldImage == null)
                    return false;

                bool updated = oldImage.Update(newImage.ImageUrl, newImage.UpdatedBy);
                if (updated)
                {
                    _db.ProductImageUrls.Update(oldImage);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ToggleDeleteStatusAsync(int id, string userModified)
        {
            try
            {
                var img = await _db.ProductImageUrls.FindAsync(id);
                if (img == null)
                    return false;

                bool result = img.ToggleDelete(userModified);
                if (result)
                {
                    _db.ProductImageUrls.Update(img);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

