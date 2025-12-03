
using Ecom.BLL.ModelVM.Cart;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;

namespace Ecom.BLL.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;

        public AccountService(
            IAccountRepo accountRepo,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ITokenService tokenService,
            ICartService cartService,
            IEmailService emailService)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _cartService = cartService;
            _emailService = emailService;
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
                string? uploadedImageUrl = null;
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
                //user.EmailConfirmed = true; // For demo purposes, set email as confirmed

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

                //6- Call the cart service to create a cart for the new user
                var addCartVM = new AddCartVM
                {
                    AppUserId = user.Id,
                    CreatedBy = user.Email!
                };
                var createCartResult = await _cartService.AddAsync(addCartVM);
                if (!createCartResult.IsSuccess)
                {
                    return new ResponseResult<AuthResponseVM>(null, "User created but failed to create cart: " + createCartResult.ErrorMessage, false);
                }

                // A. Generate the token
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // B. Encode it safely for URL
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));
                // C. Build the link pointing to your Angular Frontend
                // Example: https://localhost:4200/auth/confirm-email?userId=...&code=...
                var clientUrl = "http://localhost:4200";
                var callbackUrl = $"{clientUrl}/account/confirm-email?userId={user.Id}&code={encodedCode}";

                // D. Send Email
                await _emailService.SendEmailAsync(user.Email!, "Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                //7- If successful, generate token, and return AuthResponseVM
                var token = await _tokenService.CreateToken(user); // Generate JWT token
                var userVM = _mapper.Map<GetUserVM>(user); // Map AppUser to GetUserVM
                var authResponse = new AuthResponseVM // Create AuthresponseVM
                {
                    User = userVM,
                    Token = token,
                    TokenExpiration = DateTime.UtcNow.AddDays(7)
                };
                return new ResponseResult<AuthResponseVM>(null, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<AuthResponseVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> ConfirmEmailAsync(string userId, string code)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<bool>(false, "User not found", false);
                }

                // Decode the token back
                var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

                var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

                if (result.Succeeded)
                {
                    return new ResponseResult<bool>(true, null, true);
                }

                return new ResponseResult<bool>(false, "Invalid token", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<AuthResponseVM>> LoginAsync(LoginUserVM loginUserVM)
        {
            try
            {
                //var x = await _userManager.FindByEmailAsync("fadytawadrous3@yahoo.com");
                //var y = await  _userManager.FindByEmailAsync("fadyfofo3@yahoo.com");
                //if (x != null)
                //    await _userManager.DeleteAsync(x);
                //if (y != null)
                //    await _userManager.DeleteAsync(y);
                //1- Check if user exists with email
                var user = await _userManager.FindByEmailAsync(loginUserVM.Email);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<AuthResponseVM>(null, "Invalid email, user doesn't exist", false);
                }

                //2- Try to sign in with email and password
                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed)
                {
                    return new ResponseResult<AuthResponseVM>(null, "Email is not confirmed. Please confirm your email before logging in.", false);
                }

                //3- If failed to sign in user, return error
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserVM.Password, false);
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


        // External Authentication
        public ResponseResult<AuthenticationProperties> GetExternalLoginProperties(string provider, string redirectUrl)
        {
            try
            {
                // 1- Validate authentication provider
                if (string.IsNullOrEmpty(provider))
                    return new ResponseResult<AuthenticationProperties>(null, "Provider not specified", false);

                // 2- Configure the authentication properties (provider and Backend redirectUrl after successful login)
                var authProperties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

                return new ResponseResult<AuthenticationProperties>(authProperties, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<AuthenticationProperties>(null, ex.Message, false);
            }
        }


        public async Task<ResponseResult<AuthResponseVM>> ExternalLoginCallbackAsync()
        {
            try
            {
                // 1. Get external login info from the provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return new ResponseResult<AuthResponseVM>(null, "External login failed", false);

                // 2. Check if a user already exists with this login provider
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    // 3. If not, create a new AppUser using info from the provider
                    string email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? Guid.NewGuid().ToString() + "@temp.com";
                    string displayName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "New User";
                    string? uploadedImageUrl = null; // use default image

                    user = new AppUser(email, displayName, uploadedImageUrl, email, null); // created by same user's email
                    user.EmailConfirmed = true;

                    // 4. Create user in Identity
                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                        return new ResponseResult<AuthResponseVM>(null, errors, false);
                    }

                    // 5. Add external login info
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        var errors = string.Join(", ", addLoginResult.Errors.Select(e => e.Description));
                        return new ResponseResult<AuthResponseVM>(null, errors, false);
                    }

                    // 6. Assign default role to the new user
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "Customer");
                    if (!addToRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                        return new ResponseResult<AuthResponseVM>(null, errors, false);
                    }

                    // 7. Call the cart service to create a cart for the new user
                    var addCartVM = new AddCartVM
                    {
                        AppUserId = user.Id,
                        CreatedBy = user.Email!
                    };
                    var createCartResult = await _cartService.AddAsync(addCartVM);
                    if (!createCartResult.IsSuccess)
                    {
                        return new ResponseResult<AuthResponseVM>(null, "User created but failed to create cart: " + createCartResult.ErrorMessage, false);
                    }                    
                }                

                // Reactivate user if previously deleted
                if (user.IsDeleted)
                {
                    user.ToggleDelete(user.Email);
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return new ResponseResult<AuthResponseVM>(null, errors, false);
                    }
                }

                // Generate JWT token using your TokenService
                var token = await _tokenService.CreateToken(user);

                // Map AppUser to GetUserVM
                var userVM = _mapper.Map<GetUserVM>(user);

                // Return AuthResponseVM
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

    }
}
