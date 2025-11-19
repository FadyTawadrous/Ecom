
namespace Ecom.DAL.Repo.Implementation
{
    public class PaymentRepo 
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Payment>> GetAllAsync(Expression<Func<Payment, bool>>? Filter = null,
        //    params Expression<Func<Payment, object>>[] includes)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public async Task<Payment?> GetByIdAsync(int id)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    return null;
                }
                var payment = await _context.Payments.Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.OrderId == orderId);
                return payment;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AddAsync(Payment payment)
        {
            try
            {
                if (payment == null)
                {
                    return false;
                }

                var result = await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public async Task<bool> UpdateAsync(Payment payment)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public async Task<bool> ToggleDeleteStatusAsync(int id, string userModified)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        
    }
}
