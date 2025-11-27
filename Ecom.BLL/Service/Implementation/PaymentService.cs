using Ecom.BLL.ModelVM.Product;
using Ecom.DAL.Enum;

namespace Ecom.BLL.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public PaymentService(IPaymentRepo paymentRepo, IMapper mapper, OrderService orderService, ProductService productService)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<ResponseResult<Payment>> CreatePaymentRecordAsync(CreatePaymentVM paymentVM, string userId)
        {
            try
            {
                // 1. Validate Order exists and belongs to user (user Id fom JWT)
                var result = await _orderService.GetByIdAsync(paymentVM.OrderId);
                if (!result.IsSuccess || result.Result == null)
                {
                    return new ResponseResult<Payment>(null, "Order not found", false);
                }
                var order = result.Result;
                if (order.AppUserId != userId)
                {
                    return new ResponseResult<Payment>(null, "Order doesn't belong to this user", false);
                }

                // 2. Check if payment already exists
                var existingPayment = await _paymentRepo.GetByOrderIdAsync(paymentVM.OrderId);
                if (existingPayment != null)
                {
                    return new ResponseResult<Payment>(existingPayment, "Payment already initiated.", true);
                }

                // 3. Create new Payment Entity
                // We use the Order's TotalAmount. Never trust the frontend for amounts!
                var payment = new Payment(
                    orderId: order.Id,
                    totalamount: order.TotalAmount, // Security: Use DB amount, not Frontend amount
                    paymentMethod: paymentVM.PaymentMethod,
                    transactionId: null,
                    createdBy: userId
                );

                // 4. Save to DB
                var success = await _paymentRepo.AddAsync(payment);
                if (!success)
                {
                    return new ResponseResult<Payment>(null, "Failed to create payment record.", false);
                }

                return new ResponseResult<Payment>(payment, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<Payment>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> UpdatePaymentStatusAsync(PaymentResultVM model, string updatedBy)
        {
            try
            {
                // 1. Get the payment
                var payment = await _paymentRepo.GetByIdAsync(model.PaymentId);
                if (payment == null)
                {
                    return new ResponseResult<bool>(false, "Payment not found.", false);
                }

                //2. Get the associated order
                var orderResponse = await _orderService.GetByIdAsync(payment.OrderId);
                var order = orderResponse.Result;
                if (order == null)
                {
                    return new ResponseResult<bool>(false, "Associated order not found.", false);
                }

                // 3. Update payment status and transaction Id
                var isUpdateSuccessful = payment.Update(
                    transactionId: model.TransactionId,
                    userModified: updatedBy,
                    paymentStatus: model.Status
                );
                if (!isUpdateSuccessful)
                {
                    return new ResponseResult<bool>(false, "No changes detected to update payment.", false);
                }

                // 4. Update the Order Status, and Product QuantitySold based on Payment Status
                // If Payment is Completed
                if (model.Status == PaymentStatus.Completed)
                {
                    var updateOrderResponse = await _orderService.UpdateStatusAsync(payment.OrderId, OrderStatus.Processing, updatedBy);
                    if (!updateOrderResponse.IsSuccess)
                    {
                        return updateOrderResponse;
                    }
                    // Increase QuantitySold for each product in the order
                    foreach (var item in order.Items)
                    {
                        var addQuantityVM = new AddQuantitySoldVM
                        {
                            ProductId = item.ProductId,
                            QuantitySold = item.Quantity
                        };
                        var productUpdateResult = await _productService.AddToQuantitySoldAsync(addQuantityVM);
                        if (!productUpdateResult.IsSuccess)
                        {
                            return new ResponseResult<bool>(false, $"Failed to update quantity sold for product ID {item.ProductId}.", false);
                        }
                    }
                }
                // If Payment failed
                else if (model.Status == PaymentStatus.Failed)
                {
                    var updateOrderResponse = await _orderService.UpdateStatusAsync(payment.OrderId, OrderStatus.Pending, updatedBy);
                    if (!updateOrderResponse.IsSuccess)
                    {
                        return updateOrderResponse;
                    }
                    // Increase QuantitySold for each product in the order
                    //foreach (var item in order.Items)
                    //{
                    //    var productUpdateResult = await _productService.IncreaseStockAsync(item.ProductId, item.Quantity);
                    //    if (!productUpdateResult.IsSuccess)
                    //    {
                    //        return new ResponseResult<bool>(false, $"Failed to update quantity sold for product ID {item.ProductId}.", false);
                    //    }
                    //}
                }

                var updateResult = await _paymentRepo.UpdateAsync(payment);
                if (!updateResult)
                {
                    return new ResponseResult<bool>(false, "Failed to update payment status (invalid data).", false);
                }

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<Payment>> GetPaymentByOrderIdAsync(int orderId)
        {
            if (orderId <= 0)
            {
                return new ResponseResult<Payment>(null, "Invalid Order ID.", false);
            }

            var payment = await _paymentRepo.GetByOrderIdAsync(orderId);
            if (payment == null)
            {
                return new ResponseResult<Payment>(null, "Payment not found.", false);
            }

            return new ResponseResult<Payment>(payment, null, true);
        }

        public async Task<ResponseResult<IEnumerable<GetPaymentVM>>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _paymentRepo.GetAllAsync(p => !p.IsDeleted, includes: e => e.Order);
                var mappedPayments = _mapper.Map<IEnumerable<GetPaymentVM>>(payments);

                return new ResponseResult<IEnumerable<GetPaymentVM>>(mappedPayments, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetPaymentVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> ToggleDeleteStatusAsync(int id, string userModified)
        {
            var result = await _paymentRepo.ToggleDeleteStatusAsync(id, userModified);
            return result
                ? new ResponseResult<bool>(true, null, true)
                : new ResponseResult<bool>(false, "Failed to toggle delete status.", false);
        }


    }
}
