
using Ecom.BLL.ModelVM.Order;
using Ecom.DAL.Enum;

namespace Ecom.BLL.Admin.Service.Abstraction
{
    public interface IAdminOrderService
    {
        // LIST ALL ORDERS
        Task<ResponseResult<List<GetOrderVM>>> GetAllAsync();

        // SINGLE ORDER
        Task<ResponseResult<GetOrderVM>> GetByIdAsync(int id);

        // ORDERS BY USER
        Task<ResponseResult<List<GetOrderVM>>> GetByUserIdAsync(string userId);

        // UPDATE STATUS (Pending → Processing → Shipped → Delivered, etc.)
        Task<ResponseResult<bool>> UpdateStatusAsync(int id, OrderStatus newStatus, string updatedBy);

        // CANCEL ORDER (and return stock etc. via OrderService)
        Task<ResponseResult<bool>> CancelOrderAsync(int id, string cancelledBy);

        // SOFT DELETE
        Task<ResponseResult<bool>> DeleteAsync(int id, string deletedBy);
    }
}
