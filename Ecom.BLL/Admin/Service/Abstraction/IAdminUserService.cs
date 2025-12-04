
namespace Ecom.BLL.Admin.Service.Abstraction
{
    public interface IAdminUserService
    {
        Task<ResponseResult<IEnumerable<GetUserVM>>> GetAllAsync();
        Task<ResponseResult<GetUserVM>> GetByIdAsync(string userId);
        Task<ResponseResult<bool>> SoftDeleteAsync(string userId, string adminId);
    }
}
