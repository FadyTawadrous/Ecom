
namespace Ecom.BLL.Service.Abstraction
{
    public interface IRoleService
    {
        Task<ResponseResult<RoleVM>> CreateRoleAsync(CreateRoleVM createRoleVM);
        Task<ResponseResult<IEnumerable<RoleVM>>> GetAllRolesAsync();
        Task<ResponseResult<IEnumerable<string>>> GetUserRolesAsync(string userId);
        Task<ResponseResult<bool>> DeleteRoleAsync(string roleName);
        Task<ResponseResult<bool>> UpdateUserRolesAsync(UserRolesVM userRolesVM);


    }
}
