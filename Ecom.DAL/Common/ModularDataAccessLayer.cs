
using Ecom.DAL.Repo.Implementation;

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
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders() // Add all default token providers
                .AddSignInManager<SignInManager<AppUser>>();
                //.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);


            // Add Identity service with cookie, later will be changed for JWT
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/Login");

                    // Cookie settings, added for security
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            services.AddScoped<IProductImageUrlRepo, ProductImageUrlRepo>();
            //Dependency injection s When a controller or service asks for an IProductImageUrlRepo,
            // give them a new ProductImageUrlRepo instance for each HTTP request
            services.AddScoped<IBrandRepo, BrandRepo>();

            services.AddScoped<IProductRepo, ProductRepo>();

            services.AddScoped<IAccountRepo, AccountRepo>();
            return services;
        }
    }
}
