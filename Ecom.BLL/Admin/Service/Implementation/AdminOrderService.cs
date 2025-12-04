
using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.BLL.ModelVM.Order;
using Ecom.DAL.Enum;

namespace Ecom.BLL.Admin.Service.Implementation
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly IOrderService _orderService;

        public AdminOrderService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<ResponseResult<List<GetOrderVM>>> GetAllAsync()
        {
            // This ensures admin gets the full list of orders
            return _orderService.GetAllAsync();
        }

        public Task<ResponseResult<GetOrderVM>> GetByIdAsync(int id)
        {
            return _orderService.GetByIdAsync(id);
        }

        public Task<ResponseResult<List<GetOrderVM>>> GetByUserIdAsync(string userId)
        {
            return _orderService.GetOrdersByUserIdAsync(userId);
        }

        public Task<ResponseResult<bool>> UpdateStatusAsync(int id, OrderStatus newStatus, string updatedBy)
        {
            // This already validates IsValidStatusTransition in domain (Order.ChangeStatus)
            return _orderService.UpdateStatusAsync(id, newStatus, updatedBy);
        }

        public Task<ResponseResult<bool>> CancelOrderAsync(int id, string cancelledBy)
        {
            // Uses your existing CancelOrderAsync logic:
            // - Set status to Cancelled
            // - Return stock to Product
            return _orderService.CancelOrderAsync(id, cancelledBy);
        }

        public Task<ResponseResult<bool>> DeleteAsync(int id, string deletedBy)
        {
            return _orderService.DeleteAsync(id, deletedBy);
        }
    }
}
