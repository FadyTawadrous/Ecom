
using Microsoft.AspNetCore.Authentication;

namespace Ecom.BLL.Service.Abstraction
{
    public interface IAccountService
    {
        // 1- Register
        Task<ResponseResult<AuthResponseVM>> RegisterAsync(RegisterUserVM registerVM);
        Task<ResponseResult<bool>> ConfirmEmailAsync(string userId, string code);

        // 2- Login
        Task<ResponseResult<AuthResponseVM>> LoginAsync(LoginUserVM loginVM);

        // 3- Update
        // The 'userId' will come from the JWT token in the controller.
        Task<ResponseResult<GetUserVM>> UpdateProfileAsync(string userId, UpdateUserVM updateVM);

        // 4- get all users thats not deleted (Admin Function)
        Task<ResponseResult<IEnumerable<GetUserVM>>> GetAllUsersAsync();

        // 5- get user by Id (Admin Function)
        Task<ResponseResult<GetUserVM>> GetUserByIdAsync(string userId);

        // 6- get current logged in user
        Task<ResponseResult<GetUserVM>> GetCurrentUserAsync(ClaimsPrincipal userPrincipal);
        // ClaimsPrincipal allows you to extract the current user from the JWT or cookie content

        // 7- delete (soft delete) (Admin Function)
        // The 'userModifiedId' is the ID of the admin performing the delete.
        Task<ResponseResult<bool>> DeleteUserAsync(string userId, string modifiedBy);

        // 8- Sign out
        // For a stateless JWT-based API, SignOut is a client-side action
        // (the Angular app deletes the token).
        // If you were using server-side sessions or refresh tokens,
        // we would add a 'LogoutAsync()' method here to revoke them.
        // For now, it's not needed in the BLL.
        Task<ResponseResult<bool>> SignOutAsync();

        // Change Password
        //Task<ResponseResult<bool>> ChangePasswordAsync(string userId, ChangePasswordVM model);

        // External Login
        ResponseResult<AuthenticationProperties> GetExternalLoginProperties(string provider, string returnUrl);

        // External Login Callback
        Task<ResponseResult<AuthResponseVM>> ExternalLoginCallbackAsync();
    }
}
