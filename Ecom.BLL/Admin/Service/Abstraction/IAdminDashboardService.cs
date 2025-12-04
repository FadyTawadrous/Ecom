
using Ecom.BLL.Admin.ModelVM;

namespace Ecom.BLL.Admin.Service.Abstraction
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardVM> GetOverviewAsync();
    }
}
