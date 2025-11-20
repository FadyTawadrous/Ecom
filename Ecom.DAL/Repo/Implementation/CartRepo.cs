
namespace Ecom.DAL.Repo.Implementation
{
    // Implementation of Cart Repository
    public class CartRepo : ICartRepo
    {
        // Dependency Injection of ApplicationDbContext 
        private readonly ApplicationDbContext _db;
        public CartRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        // Add a new Cart
        // Returns true if addition is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> AddAsync(Cart newCart)
        {
            try
            {
                if (newCart == null)
                {
                    return false;
                }
                var result = await _db.Carts.AddAsync(newCart);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Get Cart by ID with optional including related entities
        // Returns the Cart entity if found
        // Throws exception if an error occurs
        public async Task<Cart> GetByIdAsync(int id, params Expression<Func<Cart, object>>[] includes)
        {
            try
            {
                // Build the query with includes
                IQueryable<Cart> query = _db.Carts.Where(c => c.Id == id && !c.IsDeleted);

                // Include related entities
                foreach (var include in includes) 
                {
                    query = query.Include(include);
                }

                // Execute the query to find the cart by Id
                var cart = await query.FirstOrDefaultAsync();
                if (cart != null)
                {
                    return cart;
                }
                throw new KeyNotFoundException($"Cart with Id {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Get Cart by UserId with optional including related entities
        // Returns the Cart entity if found
        // Throws exception if an error occurs
        public async Task<Cart> GetByUserIdAsync(string userId, params Expression<Func<Cart, object>>[] includes)
        {
            try
            {
                // Build the query with includes
                IQueryable<Cart> query = _db.Carts.Where(c => c.AppUserId == userId && !c.IsDeleted);

                // Include related entities
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                // Execute the query to find the cart by UserId
                var cart = await query.FirstOrDefaultAsync();
                if (cart != null)
                {
                    return cart;
                }
                throw new KeyNotFoundException($"Cart for UserId {userId} not found.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ClearCartAsync(int cartId)
        {
            try
            {
                var cartItems = await _db.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();

                if (cartItems.Count > 0)
                {
                    _db.CartItems.RemoveRange(cartItems);
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
