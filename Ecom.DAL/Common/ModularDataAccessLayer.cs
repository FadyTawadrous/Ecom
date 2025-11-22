using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Ecom.DAL.Entity;
// Note: Ensure you have the correct using statements for your specific Repo classes

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

            services.AddScoped<IProductImageUrlRepo, ProductImageUrlRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IProductReviewRepo, ProductReviewRepo>();

            // Dependency injection: When a controller or service asks for an IBrandRepo,
            // give them a new BrandRepo instance for each HTTP request
            services.AddScoped<IBrandRepo, BrandRepo>();
            
            services.AddScoped<IOrderRepo, OrderRepo>();

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