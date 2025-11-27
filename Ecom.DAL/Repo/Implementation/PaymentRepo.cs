
using Ecom.DAL.Entity;

namespace Ecom.DAL.Repo.Implementation
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(Expression<Func<Payment, bool>>? filter = null,
            params Expression<Func<Payment, object>>[] includes)
        {
            try
            {
                IQueryable<Payment> query = _context.Payments;

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

        public async Task<Payment?> GetByIdAsync(int id)
        {
            try
            {
                var payment = await _context.Payments.Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (payment != null)
                {
                    return payment;
                }

                throw new KeyNotFoundException($"Payment with Id {id} not found.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            try
            {
                var payment = await _context.Payments.Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.OrderId == orderId);
                if (payment != null)
                {
                    return payment;
                }

                throw new KeyNotFoundException($"Payment with orderId {orderId} not found.");
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

        public async Task<bool> UpdateAsync(Payment newPayment)
        {
            try
            {
                _context.Payments.Update(newPayment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> ToggleDeleteStatusAsync(int id, string userModified)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                {
                    return false;
                }
                bool result = payment.ToggleDelete(userModified);
                if (result)
                {
                    await _context.SaveChangesAsync();
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
