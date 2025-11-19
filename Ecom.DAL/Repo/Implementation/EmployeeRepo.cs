//using Tiers.DAL.Repo.Abstraction;

//namespace Tiers.DAL.Repo.Implementation
//{
//    public class EmployeeRepo : IEmployeeRepo
//    {
//        private readonly ApplicationDbContext _db;

//        public EmployeeRepo(ApplicationDbContext context)
//        {
//            _db = context;
//        }

//        public async Task<IEnumerable<Employee>> GetAllAsync(Expression<Func<Employee, bool>>? filter = null,
//            params Expression<Func<Employee, object>>[] includes)
//        {
//            try
//            {
//                IQueryable<Employee> query = _db.Employees;

//                if (filter != null)
//                {
//                    query = query.Where(filter);
//                }

//                foreach (var include in includes)
//                {
//                    query = query.Include(include);
//                }

//                return await query.ToListAsync();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<Employee?> GetByIdAsync(int id)
//        {
//            try
//            {
//                var emp = await _db.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);
//                if (emp != null)
//                {
//                    return emp;
//                }
//                throw new KeyNotFoundException($"Employee with Id {id} not found.");
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<bool> AddAsync(Employee newEmployee)
//        {
//            try
//            {
//                if (newEmployee == null)
//                {
//                    return false;
//                }
//                var result = await _db.Employees.AddAsync(newEmployee);
//                await _db.SaveChangesAsync();
//                return result.Entity.Id > 0;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<bool> UpdateAsync(Employee newEmployee)
//        {
//            try
//            {
//                if (newEmployee == null)
//                {
//                    return false;
//                }
//                var oldEmployee = await _db.Employees.FindAsync(newEmployee.Id);
//                if (oldEmployee == null)
//                {
//                    return false;
//                }
//                bool result = oldEmployee.Update(newEmployee.Name, newEmployee.Salary, newEmployee.DepartmentId, newEmployee.UpdatedBy, newEmployee.Age, newEmployee.ImageUrl);
//                if (result)
//                {
//                    await _db.SaveChangesAsync();
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<bool> ToggleDeleteStatusAsync(int id, string userModified)
//        {
//            try
//            {
//                var emp = await _db.Employees.FindAsync(id);
//                if (emp == null)
//                {
//                    return false;
//                }
//                bool result = emp.ToggleDelete(userModified);
//                if (result)
//                {
//                    await _db.SaveChangesAsync();
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//    }
//}
