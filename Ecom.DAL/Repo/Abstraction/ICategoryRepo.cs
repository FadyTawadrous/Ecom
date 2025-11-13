
namespace Ecom.DAL.Repo.Abstraction
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllAsync
            (Expression<Func<Category, bool>>? filter = null,
            params Expression<Func<Category, object>>[] includes);

        Task<Category> GetByIdAsync(int id);
        Task<bool> AddAsync(Category newCategory);
        Task<bool> UpdateAsync(Category newCategory);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleDeleteAsync(int id, string userModified);
    }
}
