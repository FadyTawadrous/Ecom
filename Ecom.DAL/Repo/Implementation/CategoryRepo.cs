
using Ecom.DAL.Repo.Abstraction;

namespace Ecom.DAL.Repo.Implementation
{
    // Implementation of Category Repository
    public class CategoryRepo : ICategoryRepo
    {
        // Dependency Injection of ApplicationDbContext
        private readonly ApplicationDbContext _db;
        public CategoryRepo(ApplicationDbContext context)
        {
            _db = context;
        }
        // Add a new Category
        // Returns true if addition is successful, otherwise false
        // Throws exception if an error occurs 
        public async Task<bool> AddAsync(Category newCategory)
        {
            try
            {
                if(newCategory == null)
                {
                    return false;
                }
                var result = await _db.Categories.AddAsync(newCategory);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Delete an Existing Category
        // Returns true if Delete is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var result = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) 
                {
                    return false;
                }
                _db.Categories.Remove(result);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Get all Categories with optional filtering and including related entities
        // Returns a collection of Category entities
        // Throws exception if an error occurs
        public async Task<IEnumerable<Category>> GetAllAsync
            (Expression<Func<Category, bool>>? filter = null,
            params Expression<Func<Category, object>>[] includes)
        {
            try
            {
                IQueryable<Category> query = _db.Categories;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Get a Category by its Id
        // Returns the Category entity if found
        // Throws KeyNotFoundException if not found
        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if(category != null)
                {
                    return category;
                }
                throw new KeyNotFoundException($"Category with Id {id} not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Toggle the deletion status of a Category
        // Returns true if the operation is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> ToggleDeleteAsync(int id, string userModified)
        {
            try
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                {
                    return false;
                }
                bool result = category.ToggleDelete(userModified);
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

        // Update an existing Category
        // Returns true if the update is successful, otherwise false
        // Throws exception if an error occurs
        public async Task<bool> UpdateAsync(Category newCategory)
        {
            try
            {
                if (newCategory == null)
                {
                    return false;
                }
                var oldCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == newCategory.Id);
                if (oldCategory == null)
                {
                    return false;
                }
                bool result = oldCategory.Update(newCategory.Name, newCategory.ImageUrl, newCategory.UpdatedBy);
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
