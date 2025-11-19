
namespace Ecom.DAL.Repo.Implementation
{
    public class BrandRepo : IBrandRepo
    {
        private readonly ApplicationDbContext _context;

        public BrandRepo(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? filter = null)
        {
            IQueryable<Brand> query = _context.Brands;
            if (filter != null)
                query = query.Where(filter);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, string deletedBy)
        {
            var brand = await GetByIdAsync(id);
            if (brand != null)
            {
                brand.ToggleDelete(deletedBy);
                _context.Brands.Update(brand);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
