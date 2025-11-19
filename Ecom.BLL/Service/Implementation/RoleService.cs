
namespace Ecom.BLL.Service.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ResponseResult<RoleVM>> CreateRoleAsync(CreateRoleVM createRoleVM)
        {
            try
            {
                //1- Check if role already exists
                if (await _roleManager.RoleExistsAsync(createRoleVM.RoleName))
                {
                    return new ResponseResult<RoleVM>(null, "Role already exists.", false);
                }

                //2- Create a new role
                var newRole = new IdentityRole(createRoleVM.RoleName);
                var result = await _roleManager.CreateAsync(newRole);

                //3- If creation failed, return errors
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ResponseResult<RoleVM>(null, errors, false);
                }

                //4- Map the new role to RoleVM and return success
                var roleVM = _mapper.Map<RoleVM>(newRole);
                return new ResponseResult<RoleVM>(roleVM, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<RoleVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteRoleAsync(string roleName)
        {
            try
            {
                //1- Find the role
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new ResponseResult<bool>(false, "Role not found.", false);
                }

                //2- Prevent deletion of core system roles
                if (role.Name == "Admin" || role.Name == "Customer")
                {
                    return new ResponseResult<bool>(false, "Cannot delete core system roles.", false);
                }

                //3- Delete the role
                var result = await _roleManager.DeleteAsync(role);
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

        public async Task<ResponseResult<IEnumerable<RoleVM>>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var rolesVM = _mapper.Map<IEnumerable<RoleVM>>(roles);
                return new ResponseResult<IEnumerable<RoleVM>>(rolesVM, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<RoleVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<string>>> GetUserRolesAsync(string userId)
        {
            try
            {
                //1- Check if user exists
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<IEnumerable<string>>(null, "User not found.", false);
                }

                //2- Get user roles
                var roles = await _userManager.GetRolesAsync(user);
                return new ResponseResult<IEnumerable<string>>(roles, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<string>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> UpdateUserRolesAsync(UserRolesVM userRolesVM)
        {
            try
            {
                //1- Check if user exists
                var user = await _userManager.FindByIdAsync(userRolesVM.UserId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseResult<bool>(false, "User not found or deleted.", false);
                }

                //2- Get current user roles
                var currentRoles = await _userManager.GetRolesAsync(user);

                //3- Calculate roles to add (new roles)
                var rolesToAdd = userRolesVM.RoleNames.Except(currentRoles).ToList();
                if (rolesToAdd.Count > 0)
                {
                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                        return new ResponseResult<bool>(false, errors, false);
                    }
                }

                //4- Calculate roles to remove
                var rolesToRemove = currentRoles.Except(userRolesVM.RoleNames).ToList();
                if (rolesToRemove.Count > 0)
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        return new ResponseResult<bool>(false, errors, false);
                    }
                }

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }


    }
}
