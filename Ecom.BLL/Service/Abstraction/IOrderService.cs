using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.BLL.ModelVM.Order;
using Ecom.BLL.ModelVM.OrderItem;
using Ecom.DAL.Enum;

namespace Ecom.BLL.Service.Abstraction
{
    public interface IOrderService
    {
        // CREATE
        Task<ResponseResult<GetOrderVM>> CreateOrderAsync(string userId, string shippingAddress);

        // READ
        Task<ResponseResult<GetOrderVM>> GetByIdAsync(int id);
        Task<ResponseResult<List<GetOrderVM>>> GetAllAsync();
        Task<ResponseResult<List<GetOrderVM>>> GetOrdersByUserIdAsync(string userId);

        // FILTERING
        //Task<ResponseResult<List<GetOrderVM>>> FilterAsync(OrderFilterVM filter);

        // UPDATE STATUS
        Task<ResponseResult<bool>> UpdateStatusAsync(int id, OrderStatus newStatus, string updatedBy);

        
        // DELETE (soft delete)
        Task<ResponseResult<bool>> DeleteAsync(int id, string userId);

        Task<ResponseResult<bool>> CancelOrderAsync(int id, string userId);
        
    }

}
