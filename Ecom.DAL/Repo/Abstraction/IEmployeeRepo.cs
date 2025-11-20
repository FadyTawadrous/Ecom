//namespace Tiers.DAL.Repo.Abstraction
//{
//    public interface IEmployeeRepo
//    {
//        // Query Methods
//        Task<Employee?> GetByIdAsync(int id);
//        Task<IEnumerable<Employee>> GetAllAsync(
//            Expression<Func<Employee, bool>>? Filter = null,
//            params Expression<Func<Employee, object>>[] includes);

//        // Command Methods
//        Task<bool> AddAsync(Employee newEmployee);
//        Task<bool> UpdateAsync(Employee newEmployee);
//        Task<bool> ToggleDeleteStatusAsync(int id, string userModified);
//    }
//}
