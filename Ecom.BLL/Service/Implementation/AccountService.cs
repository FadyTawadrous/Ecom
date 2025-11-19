
namespace Ecom.BLL.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ResponseResult<AuthResponseVM>> RegisterAsync(RegisterUserVM registerVM)
        {
            try
            {
                //1- Check if email already exists
                if (await _userManager.FindByEmailAsync(registerVM.Email) != null)
                {
                    return new ResponseResult<AuthResponseVM>(null, "Email address is already in use.", false);
                }

                //2- Handle file upload
                string? uploadedImageUrl = "default.png";
                if (registerVM.ProfileImage != null)
                {
                    try
                    {
                        uploadedImageUrl = await Upload.UploadFileAsync("Images/UserImages", registerVM.ProfileImage);
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<AuthResponseVM>(null, $"File upload failed: {ex.Message}", false);
                    }
                }
                registerVM.ProfileImageUrl = uploadedImageUrl;

                //3- Map RegisterUserVM to AppUser
                var user = _mapper.Map<AppUser>(registerVM);

                //4 - Add the new user using UserManager
                user.EmailConfirmed = true; // For demo purposes, set email as confirmed

                var createResult = await _userManager.CreateAsync(user, registerVM.Password);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return new ResponseResult<AuthResponseVM>(null, errors, false);
                }

                //5- Assign "Customer" role to the new user
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Customer");
                if (!addToRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                    return new ResponseResult<AuthResponseVM>(null, errors, false);
                }

                // Here you should also call the cart service to create a cart for the user

                //6- If successful, generate token, and return AuthResponseVM
                var token = await _tokenService.CreateToken(user); // Generate JWT token
                var userVM = _mapper.Map<GetUserVM>(user); // Map AppUser to GetUserVM
                var authResponse = new AuthResponseVM // Create AuthresponseVM
                {
                    User = userVM,
                    Token = token,
                    TokenExpiration = DateTime.UtcNow.AddDays(7)
                };
                return new ResponseResult<AuthResponseVM>(authResponse, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<AuthResponseVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<AuthResponseVM>> LoginAsync(LoginUserVM loginUserVM)
        {
            try
            {
                //1- Check if user exists with email
                var user = await _userManager.FindByEmailAsync(loginUserVM.Email);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<AuthResponseVM>(null, "Invalid email, user doesn't exist", false);
                }

                //2- Try to sign in with email and password
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserVM.Password, false);

                //3- If failed to sign in user, return error
                if (!result.Succeeded)
                {
                    return new ResponseResult<AuthResponseVM>(null, "Invalid password.", false);
                }

                //4- If successful, generate token, and return AuthResponseVM
                var token = await _tokenService.CreateToken(user); // Generate JWT token
                var userVM = _mapper.Map<GetUserVM>(user); // Map AppUser to GetUserVM
                var authResponse = new AuthResponseVM // Create AuthresponseVM
                {
                    User = userVM,
                    Token = token,
                    TokenExpiration = DateTime.UtcNow.AddDays(7)
                };
                return new ResponseResult<AuthResponseVM>(authResponse, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<AuthResponseVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetUserVM>> UpdateProfileAsync(string userId, UpdateUserVM updateVM)
        {
            try
            {
                //1- Check if user exists
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<GetUserVM>(null, "User not found.", false);
                }

                //2- Handle file upload
                string? uploadedImageUrl = user.ProfileImageUrl; // The old image url
                if (updateVM.ProfileImage != null)
                {
                    try
                    {
                        uploadedImageUrl = await Upload.UploadFileAsync("Images/UserImages", updateVM.ProfileImage);
                        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                        {
                            await Upload.RemoveFileAsync("Images/UserImages", user.ProfileImageUrl); // Remove old image if exists
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<GetUserVM>(null, $"File upload failed: {ex.Message}", false);
                    }
                }
                updateVM.ProfileImageUrl = uploadedImageUrl;

                //3- Update user properties using Update method, and Identity methods for identity properties
                bool updateProfileResult = user.Update(updateVM.DisplayName, updateVM.ProfileImageUrl, userId);
                if (!updateProfileResult)
                {
                    return new ResponseResult<GetUserVM>(null, "Failed to update Name or Image.", false);
                }
                await _userManager.SetPhoneNumberAsync(user, updateVM.PhoneNumber);

                //4- Save changes
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ResponseResult<GetUserVM>(null, errors, false);
                }

                //5- Return updated user info
                var updatedUserVM = _mapper.Map<GetUserVM>(user);
                return new ResponseResult<GetUserVM>(updatedUserVM, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetUserVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetUserVM>>> GetAllUsersAsync() // (Admin)
        {
            try
            {
                var users = await _userManager.Users
                    .Where(u => !u.IsDeleted)
                    .ToListAsync();

                var usersVM = _mapper.Map<IEnumerable<GetUserVM>>(users);
                return new ResponseResult<IEnumerable<GetUserVM>>(usersVM, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetUserVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetUserVM>> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<GetUserVM>(null, "User not found.", false);
                }

                var userVM = _mapper.Map<GetUserVM>(user);
                return new ResponseResult<GetUserVM>(userVM, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetUserVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetUserVM>> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            try
            {
                var userId = _userManager.GetUserId(userPrincipal);
                if (string.IsNullOrEmpty(userId))
                {
                    return new ResponseResult<GetUserVM>(null, "User is not authenticated.", false);
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<GetUserVM>(null, "User not found.", false);
                }

                var userVM = _mapper.Map<GetUserVM>(user);
                return new ResponseResult<GetUserVM>(userVM, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetUserVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteUserAsync(string userId, string modifiedBy)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<bool>(false, "User not found.", false);
                }

                user.ToggleDelete(modifiedBy);
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ResponseResult<bool>(false, errors, false);
                }

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> SignOutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(true, ex.Message, false);
            }
        }


    }
}
