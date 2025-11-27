
using Azure;
using Microsoft.AspNetCore.Identity;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // 1. Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseVM>> Register([FromForm] RegisterUserVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.RegisterAsync(registerVM);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.ErrorMessage });
                }

                return Ok(response.Result);
            }
            return BadRequest(ModelState);
        }

        // 2. Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseVM>> Login([FromBody] LoginUserVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.LoginAsync(loginVM);

                if (!response.IsSuccess)
                {
                    return Unauthorized(new { message = response.ErrorMessage });
                }
                return Ok(response.Result);
            }
            return BadRequest(ModelState);
        }

        // 3. Update Profile
        [Authorize] // User must be logged in
        [HttpPut("profile")]
        public async Task<ActionResult<GetUserVM>> UpdateProfile([FromForm] UpdateUserVM updateVM)
        {
            if (ModelState.IsValid)
            {
                if (CurrentUserId == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                var response = await _accountService.UpdateProfileAsync(CurrentUserId, updateVM);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.ErrorMessage });
                }
                return Ok(response.Result);
            }
            return BadRequest(ModelState);
        }

        // 4. Get All Users (Admin)
        [Authorize(Roles = "Admin")] // Must be an Admin
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<GetUserVM>>> GetAllUsers()
        {
            var response = await _accountService.GetAllUsersAsync();
            if (!response.IsSuccess)
            {
                return NotFound(new { message = response.ErrorMessage });
            }
            return Ok(response.Result); // Will be an empty list if no users
        }

        // 5. Get User By Id (Admin)
        [Authorize(Roles = "Admin")] // Must be an Admin
        [HttpGet("users/{id}")]
        public async Task<ActionResult<GetUserVM>> GetUserById(string id)
        {
            var response = await _accountService.GetUserByIdAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(new { message = response.ErrorMessage });
            }
            return Ok(response.Result);
        }

        // 6. Get Current Logged In User
        [Authorize] // Must be logged in
        [HttpGet("me")]
        public async Task<ActionResult<GetUserVM>> GetCurrentUser()
        {
            // We pass the controller's 'User' (ClaimsPrincipal) to the service
            var response = await _accountService.GetCurrentUserAsync(User);

            if (!response.IsSuccess)
            {
                return NotFound(new { message = response.ErrorMessage });
            }
            return Ok(response.Result);
        }

        // 7. Delete User (Admin)
        [Authorize(Roles = "Admin")] // Must be an Admin
        [HttpDelete("users/{id}")]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] string id)
        {
            if (CurrentUserId == null)
            {
                return Unauthorized(new { message = "Admin user not authenticated." });
            }

            // We pass the ID of the user to delete (from URL)
            // and the ID of the admin performing the action (from their token)
            var response = await _accountService.DeleteUserAsync(id, CurrentUserId);

            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.ErrorMessage });
            }
            return Ok(response.Result);
        }

        /// <summary>
        /// This endpoint is for Cookie-based authentication.
        /// For JWT tokens, "Sign Out" is a CLIENT-SIDE action
        /// (the Angular app deletes the token).
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout()
        {
            var response = await _accountService.SignOutAsync();
            if (!response.IsSuccess)
            {
                return BadRequest(new { message = response.ErrorMessage });
            }
            return Ok(response.Result);
        }


        // 8. External Login (Google, Facebook, Microsoft)
        [AllowAnonymous]
        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] string? returnUrl = null)
        {
            // 1- Build the callback URL (where Google/Facebook will redirect back to)
            var callbackUrl = Url.Action("ExternalLoginCallback", "Account",
                new { returnUrl }, // returnUrl passed in query parameters of URL later if any
                Request.Scheme);

            // 2- Get the authentication properties required by authentication provider
            var response = _accountService.GetExternalLoginProperties(provider, callbackUrl);
            if (!response.IsSuccess)
                return BadRequest(new { message = response.ErrorMessage });

            // 3- Challenge triggers the redirect to the external provider (Google, Facebook, etc.) login page
            return new ChallengeResult(provider, response.Result);
        }

        // Callback from external provider
        [AllowAnonymous]
        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromQuery] string? returnUrl = null)
        {
            var result = await _accountService.ExternalLoginCallbackAsync();

            if (!result.IsSuccess)
            {
                return Redirect($"{returnUrl}?error={Uri.EscapeDataString(result.ErrorMessage)}");
            }

            // Redirect to frontend with token
            var token = result.Result.Token;
            var userJson = Uri.EscapeDataString(
                System.Text.Json.JsonSerializer.Serialize(result.Result.User)
            );

            return Redirect($"{returnUrl}?token={token}&user={userJson}");
        }

    }
}
