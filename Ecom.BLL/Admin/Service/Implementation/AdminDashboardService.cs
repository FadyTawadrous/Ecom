using Ecom.BLL.Admin.ModelVM;
using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.BLL.ModelVM.Order;
using Ecom.BLL.ModelVM.Product;
using Ecom.DAL.Entity;


namespace Ecom.BLL.Admin.Service.Implementation
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<AppUser> _userManager;

        public AdminDashboardService(
            IOrderService orderService,
            IProductService productService,
            IPaymentService paymentService,
            UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _productService = productService;
            _paymentService = paymentService;
            _userManager = userManager;
        }

        public async Task<AdminDashboardVM> GetOverviewAsync()
        {
            var today = DateTime.UtcNow.Date;

            var ordersResponse = await _orderService.GetAllAsync();
            var productsResponse = await _productService.GetAllForAdminAsync();
            var paymentsResponse = await _paymentService.GetAllPaymentsAsync();

            var orders = (ordersResponse.IsSuccess && ordersResponse.Result != null
                            ? ordersResponse.Result
                            : new List<GetOrderVM>())
                         .ToList();

            var products = (productsResponse.IsSuccess && productsResponse.Result != null
                                ? productsResponse.Result
                                : new List<GetProductVM>())
                           .ToList();

            var payments = (paymentsResponse.IsSuccess && paymentsResponse.Result != null
                                ? paymentsResponse.Result
                                : new List<GetPaymentVM>())
                           .ToList();

            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.UtcNow.Date.AddDays(-i))
                .Select(day => new DailyOrdersVM
                {
                    Date = day,
                    OrdersCount = orders.Count(o => o.CreatedOn.Date == day)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new AdminDashboardVM
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalOrders = orders.Count,
                TotalProducts = products.Count,   // ✅ now works, products is List<>
                TotalRevenue = payments
                    .Where(p => p.Status.ToString() == "Completed")
                    .Sum(p => p.TotalAmount),

                TodayOrders = orders.Count(o => o.CreatedOn.Date == today),

                RecentOrders = orders
                    .OrderByDescending(o => o.CreatedOn)
                    .Take(5)
                    .Select(o => new RecentOrderVM
                    {
                        OrderId = o.Id,
                        UserName = o.CustomerName,
                        Total = o.TotalAmount,
                        Status = o.Status.ToString(),
                        CreatedOn = o.CreatedOn
                    })
                    .ToList(),

                OrdersLast7Days = last7Days
            };
        }
    }
}
