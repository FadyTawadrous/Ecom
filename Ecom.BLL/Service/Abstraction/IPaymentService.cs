
namespace Ecom.BLL.Service.Abstraction
{
    public interface IPaymentService
    {
        Task<ResponseResult<Payment>> CreatePaymentRecordAsync(CreatePaymentVM model, string userId);
        Task<ResponseResult<bool>> UpdatePaymentStatusAsync(PaymentResultVM model, string UpdatedBy);
        Task<ResponseResult<Payment>> GetPaymentByOrderIdAsync(int orderId);
        Task<ResponseResult<IEnumerable<GetPaymentVM>>> GetAllPaymentsAsync();
        Task<ResponseResult<bool>> ToggleDeleteStatusAsync(int id, string userModified);
    }
}
