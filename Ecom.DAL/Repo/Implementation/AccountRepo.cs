
namespace Ecom.DAL.Repo.Implementation
{
    public class AccountRepo : IAccountRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public AccountRepo(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public async Task<bool> AddAsync(AppUser newUser, string password)
        {
            try
            {
                if (newUser == null || string.IsNullOrEmpty(password))
                {
                    return false;
                }
                var createResult = await _userManager.CreateAsync(newUser, password);
                if (!createResult.Succeeded)
                {
                    return false;
                }
                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "Customer");
                if (!addToRoleResult.Succeeded)
                {
                    return false;
                }
                var cart = new Cart(newUser.Id, newUser.Id); // User creates their own cart
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

    }
}
