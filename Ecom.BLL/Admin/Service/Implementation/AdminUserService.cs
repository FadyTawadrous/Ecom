using Ecom.BLL.Admin.Service.Abstraction;

namespace Ecom.BLL.Admin.Service.Implementation
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAccountService _accountService;

        public AdminUserService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseResult<IEnumerable<GetUserVM>>> GetAllAsync()
        {
            return await _accountService.GetAllUsersAsync();
        }

        public async Task<ResponseResult<GetUserVM>> GetByIdAsync(string userId)
        {
            return await _accountService.GetUserByIdAsync(userId);
        }

        public async Task<ResponseResult<bool>> SoftDeleteAsync(string userId, string adminId)
        {
            return await _accountService.DeleteUserAsync(userId, adminId);
        }
    }
}
