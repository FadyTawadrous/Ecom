
namespace Ecom.BLL.Admin.ModelVM
{
    public class AdminDashboardVM
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TodayOrders { get; set; }

        public List<RecentOrderVM> RecentOrders { get; set; } = new();
        public List<DailyOrdersVM> OrdersLast7Days { get; set; } = new();
    }

    public class RecentOrderVM
    {
        public int OrderId { get; set; }
        public string? UserName { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

    }

    public class DailyOrdersVM
    {
        public DateTime Date { get; set; }
        public int OrdersCount { get; set; }
    }
}
