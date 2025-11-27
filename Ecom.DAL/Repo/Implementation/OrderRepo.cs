
namespace Ecom.DAL.Repo.Implementation
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext dbContext)
        {
            this._context = dbContext;
        }
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);     
        }

        public async Task DeleteAsync(int Id, string deletedBy)
        {
            var OrderbyId = await _context.Orders.FindAsync(Id);
            if (OrderbyId != null)
            {
                OrderbyId.ToggleDelete(deletedBy);
                _context.Orders.Update(OrderbyId);
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>>? filter = null)
        {
            IQueryable<Order> query = _context.Orders;
            if (filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Where(i => i.Id == id).Include(o => o.OrderItems).FirstOrDefaultAsync();
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order?> GetWithItemsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Order>> GetByUserIdAsync(string appUserId)
        {
            return await _context.Orders.Where(o => o.AppUserId == appUserId).ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id,string userId, OrderStatus orderStatus)
        {
            var Order = await _context.Orders.FindAsync(id);
            if (Order != null) {
                Order.Update(orderStatus, userId);
                _context.Orders.Update(Order);
            }
            await Task.CompletedTask;
        }

    }
}
