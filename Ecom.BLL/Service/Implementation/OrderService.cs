using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.BLL.ModelVM.Order;
using Ecom.BLL.ModelVM.OrderItem;
using Ecom.DAL.Entity;
using Ecom.DAL.Enum;

namespace Ecom.BLL.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo orderRepo;
        private readonly IMapper mapper;
        private readonly ICartService cartService;

        public OrderService(IOrderRepo orderRepo,IMapper mapper, ICartService cartService)
        {
            this.orderRepo = orderRepo;
            this.mapper = mapper;
            this.cartService = cartService;
        }
        //public async Task<ResponseResult<bool>> AddItemAsync(int orderId, CreateOrderItemVM itemVM)
        //{
        //    try
        //    {
        //        //var result = await orderRepo.AddAsync(orderId, itemVM);
        //        return new ResponseResult<bool>(false, null, false);

        //    }
        //    catch (Exception ex) { 
        //    return new ResponseResult<bool>(false, ex.Message, false);
        //    }
        //}

        public async Task<ResponseResult<GetOrderVM>> CreateOrderAsync(string userId,string shippingAddress)
        {
            try
            {
                // get Cart from CartService Including Items
                var result = await cartService.GetByUserIdAsync(userId);
                if (result.Result == null || result.Result.CartItems.Count == 0)
                    return new ResponseResult<GetOrderVM>(null, "Cart is empty", false);

                // Map GetCartItemsVm To OrderItems 
                
                var order = new Order(userId, DateTime.Now.AddDays(7), shippingAddress, userId, new List<OrderItem>());
                foreach (var item in result.Result.CartItems)
                {
                    var orderItem = new OrderItem(
                        item.ProductId,
                        order.Id,               // FK
                        item.Quantity,
                        item.UnitPrice,
                        userId,
                        item.ProductName        // snapshot title
                    );

                    order.AddItem(orderItem); // This recalculates total each add
                }

                // adding Order
                await orderRepo.AddAsync(order);
                await orderRepo.SaveChangesAsync();
                return new ResponseResult<GetOrderVM>(mapper.Map<GetOrderVM>(order), null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetOrderVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteAsync(int id, string userId)
        {
            try
            {
                await orderRepo.DeleteAsync(id,userId); //Soft Delete
                await orderRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, $"Order Deleted Successfuly by {userId}", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> CancelOrderAsync(int id, string userId)
        {
            try
            {
                await orderRepo.UpdateAsync(id, userId,OrderStatus.Cancelled); //Cancel Product
                await orderRepo.SaveChangesAsync();
                // add return Quatity to Stock when Product is Finished
                return new ResponseResult<bool>(true, $"Order Cancelled Successfuly by {userId}", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<List<GetOrderVM>>> GetAllAsync()
        {
            try
            {
                var result = await orderRepo.GetAllAsync();
                if (result == null)
                    return new ResponseResult<List<GetOrderVM>>(null, "Could Not Fetch Order List", false);
                var map = mapper.Map<List<GetOrderVM>>(result);
                return new ResponseResult<List<GetOrderVM>>(map, null, true);
            }
            catch (Exception ex) 
            {
                return new ResponseResult<List<GetOrderVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetOrderVM>> GetByIdAsync(int id)
        {
            try
            {
                var result = await orderRepo.GetByIdAsync(id);
                if (result == null)
                    return new ResponseResult<GetOrderVM>(null, $"Could Not Fetch Order with the id : {id}", false);
                var map = mapper.Map<GetOrderVM>(result);
                return new ResponseResult<GetOrderVM>(map, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetOrderVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<List<GetOrderVM>>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                var result = await orderRepo.GetByUserIdAsync(userId);
                if (result == null)
                    return new ResponseResult<List<GetOrderVM>>(null, $"Could Not Fetch Order with the UserId : {userId}", false);
                var map = mapper.Map<List<GetOrderVM>>(result);
                return new ResponseResult<List<GetOrderVM>>(map, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<List<GetOrderVM>>(null, ex.Message, false);
            }
        }

        //public Task<ResponseResult<bool>> RemoveItemAsync(int orderId, int itemId, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ResponseResult<bool>> UpdateItemQuantityAsync(int orderId, int itemId, int newQuantity, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ResponseResult<bool>> UpdateStatusAsync(int id, OrderStatus newStatus, string updatedBy)
        {
            try
            {
                await orderRepo.UpdateAsync(id, updatedBy, newStatus); //Update Order Status
                await orderRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, $"Order Updated Successfuly by {updatedBy}", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }


    }
}
