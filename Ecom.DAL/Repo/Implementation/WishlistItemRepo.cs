
using Ecom.BLL.Responses;

namespace Ecom.DAL.Repo.Implementation
{
    public class WishlistItemRepo : IWishlistItemRepo
    {
        private readonly ApplicationDbContext _db;

        public WishlistItemRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        // Query Methods
        public async Task<WishlistItem?> GetByIdAsync(int id, 
            params Expression<Func<WishlistItem, object>>[] includes)
        {
            try
            {
                IQueryable<WishlistItem> query = _db.WishlistItems;

                if (includes != null && includes.Any())
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                return await query.FirstOrDefaultAsync(w => w.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<WishlistItem>> GetAllByUserIdAsync(string userId,
            Expression<Func<WishlistItem, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<WishlistItem, object>>[] includes)
        {
            try
            {
                IQueryable<WishlistItem> query = _db.WishlistItems.Where(w => w.AppUserId == userId);

                // Optional filtering
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Optional eager loading
                if (includes != null && includes.Any())
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                
                // Count before pagination
                int totalCount = await query.CountAsync();

                // Apply pagination
                if (pageNumber <= 0) pageNumber = 1;
                if (pageSize <= 0) pageSize = 10;

                query = query
                    .OrderByDescending(w => w.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var items = await query.ToListAsync();
                var result = new PaginatedResult<WishlistItem>(
                    items,
                    totalCount,
                    pageNumber,
                    pageSize
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<WishlistItem>> GetAllAsync(
            Expression<Func<WishlistItem, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<WishlistItem, object>>[] includes)
        {
            try
            {
                IQueryable<WishlistItem> query = _db.WishlistItems;

                // Optional filtering
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Optional eager loading
                if (includes != null && includes.Any())
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                // Count before pagination
                int totalCount = await query.CountAsync();

                // Apply pagination
                if (pageNumber <= 0) pageNumber = 1;
                if (pageSize <= 0) pageSize = 10;

                query = query
                    .OrderByDescending(w => w.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var items = await query.ToListAsync();
                var result = new PaginatedResult<WishlistItem>(
                    items,
                    totalCount,
                    pageNumber,
                    pageSize
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Command Methods
        public async Task<bool> AddAsync(WishlistItem newItem)
        {
            try
            {
                if (newItem == null)
                {
                    return false;
                }

                // Check if the same product is already in the user's wishlist
                bool exists = await _db.WishlistItems
                    .AnyAsync(w => w.AppUserId == newItem.AppUserId && w.ProductId == newItem.ProductId);

                if (exists)
                    return true; // already exists, still return true

                var result = await _db.WishlistItems.AddAsync(newItem);

                await _db.SaveChangesAsync();

                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await _db.WishlistItems.FindAsync(id);

                if (item == null)
                {
                    return false;
                }

                _db.WishlistItems.Remove(item);

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
