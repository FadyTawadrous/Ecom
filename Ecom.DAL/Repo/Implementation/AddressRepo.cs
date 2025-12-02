
using Ecom.BLL.Responses;

namespace Ecom.DAL.Repo.Implementation
{
    public class AddressRepo : IAddressRepo
    {
        private readonly ApplicationDbContext _db;

        public AddressRepo(ApplicationDbContext context)
        {
            _db = context;
        }
        
        // Query Methods
        public async Task<Address?> GetByIdAsync(int id, 
            params Expression<Func<Address, object>>[] includes)
        {
            try
            {
                IQueryable<Address> query = _db.Addresses;

                if (includes != null && includes.Any())
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                return await query.FirstOrDefaultAsync(a => a.Id == id);          
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<Address>> GetAllByUserIdAsync(string userId,
            Expression<Func<Address, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<Address, object>>[] includes)
        {
            try
            {  
                IQueryable<Address> query = _db.Addresses.Where(a => a.AppUserId == userId);

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
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var items = await query.ToListAsync();
                var result = new PaginatedResult<Address>(
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

        public async Task<PaginatedResult<Address>> GetAllAsync(
            Expression<Func<Address, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 10,
            params Expression<Func<Address, object>>[] includes)
        {
            try
            {
                IQueryable<Address> query = _db.Addresses;

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
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var items = await query.ToListAsync();
                var result = new PaginatedResult<Address>(
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
        public async Task<bool> AddAsync(Address newAddress)
        {
            try
            {
                if (newAddress == null)
                {
                    return false;
                }

                var result = await _db.Addresses.AddAsync(newAddress);

                await _db.SaveChangesAsync();

                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Address newAddress)
        {
            try
            {
                if (newAddress == null)
                {
                    return false;
                }

                var oldAddress = await _db.Addresses.FindAsync(newAddress.Id);

                if (oldAddress == null)
                {
                    return false;
                }

                bool result = oldAddress.Update(
                    newAddress.Street, newAddress.City, newAddress.Country,
                    newAddress.PostalCode, newAddress.UpdatedBy
                    );

                if (!result)
                {
                    return false;
                }

                await _db.SaveChangesAsync();

                return true;
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
                var address = await _db.Addresses.FindAsync(id);

                if (address == null)
                {
                    return false;
                }

                bool result = address.ToggleDelete(userModified);

                if (!result)
                {
                    return false;
                }

                await _db.SaveChangesAsync();

                return true;
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
                var address = await _db.Addresses.FindAsync(id);

                if (address == null)
                {
                    return false;
                }

                _db.Addresses.Remove(address);

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
