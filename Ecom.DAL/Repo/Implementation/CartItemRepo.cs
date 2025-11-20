
namespace Ecom.DAL.Repo.Implementation
{
    // Implementation of CartItem Repository
    internal class CartItemRepo : ICartItemRepo
    {
        // Dependency Injection of ApplicationDbContext
        private readonly ApplicationDbContext _db;
        public CartItemRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // Add a new CartItem
        // Returns true if addition is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> AddAsync(CartItem newCartItem)
        {
            try
            {
                if (newCartItem == null)
                {
                    return false;
                }
                var result = await _db.CartItems.AddAsync(newCartItem);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;    
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Get a CartItem by CartId and ProductId
        // Returns the CartItem if found, otherwise throws KeyNotFoundException
        // Throws exception if an error occurs
        public async Task<CartItem> GetByCartIdAndProductIDAsync(int cartId, int productId)
        {
            try
            {
                IQueryable<CartItem> query = _db.CartItems.Where(c => c.CartId == cartId 
                                                                && c.ProductId == productId
                                                                && !c.IsDeleted);

                var cartItem = await query.FirstOrDefaultAsync();
               // if (cartItem != null)
                //{
                    return cartItem;
                //}
                //throw new KeyNotFoundException($"CartItem with CartId {cartId} and ProductId {productId} not found.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get all CartItems by CartId with optional including related entities
        // Returns a list of CartItems for the specified CartId
        // Throws exception if an error occurs
        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId, params Expression<Func<CartItem, object>>[] includes)
        {
            try
            {
                // Build the query with includes
                IQueryable<CartItem> query = _db.CartItems.Where(c => c.CartId == cartId && !c.IsDeleted);

                // Include related entities
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                // Execute the query to get CartItems by CartId
                var cartItems = await query.ToListAsync();
                if (cartItems != null)
                {
                    return cartItems;
                }
                throw new KeyNotFoundException($"No CartItems found for CartId {cartId}.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get a CartItem by its Id
        // Returns the CartItem if found, otherwise throws KeyNotFoundException
        // Throws exception if an error occurs
        public async Task<CartItem> GetByIdAsync(int id, params Expression<Func<CartItem, object>>[] includes)
        {
            try
            {
                // Build the query with includes
                IQueryable<CartItem> query = _db.CartItems.Where(c => c.Id == id && !c.IsDeleted);

                // Include related entities
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                // Execute the query to find the CartItem by Id
                var cartItem = await query.FirstOrDefaultAsync();
                if (cartItem != null)
                {
                    return cartItem;
                }
                throw new KeyNotFoundException($"CartItem with Id {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Hard Delete a CartItem by its Id
        // Returns true if deletion is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var result = await _db.CartItems.FirstOrDefaultAsync(c => c.Id == id);
                if (result == null)
                {
                    return false;
                }
                _db.CartItems.Remove(result);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update an existing CartItem
        // Returns true if update is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> UpdateAsync(CartItem newCartItem)
        {
            try
            {
                if (newCartItem == null)
                {
                    return false;
                }
                var oldCartItem = await _db.CartItems.FirstOrDefaultAsync(c => c.Id == newCartItem.Id);
                if (oldCartItem == null)
                {
                    return false;
                }
                bool result = oldCartItem.Update(newCartItem.Quantity, newCartItem.UnitPrice, newCartItem.UpdatedBy);
                if (result)
                {
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
