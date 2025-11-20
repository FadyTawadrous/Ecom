
namespace Ecom.DAL.Common
{
    public static class ModularDataAccessLayer
    {
        public static IServiceCollection AddBusinessInDAL(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // SignIn settings
                options.SignIn.RequireConfirmedAccount = true;

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings (optional)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders() // Add all default token providers
                .AddSignInManager<SignInManager<AppUser>>();
                //.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);


            

            services.AddScoped<IProductImageUrlRepo, ProductImageUrlRepo>();
            services.AddScoped<IBrandRepo, BrandRepo>();
            services.AddScoped<IAccountRepo, AccountRepo>();

            services.AddScoped<IAddressRepo, AddressRepo>();
            services.AddScoped<IWishlistItemRepo, WishlistItemRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<ICartItemRepo, CartItemRepo>();
            services.AddScoped<ICartRepo, CartRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            return services;
        }
    }
}
