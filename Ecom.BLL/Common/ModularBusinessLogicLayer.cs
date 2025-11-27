
using FaceRecognitionDotNet;

// Note: Ensure you have the correct using statements for your specific Service classes

namespace Ecom.BLL.Common
{
    public static class ModularBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessInBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // JWT Configuration
            var jwtSection = configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            })
            // External Authentication Providers
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                // Request additional scopes from Google like profile and email
                options.Scope.Add("profile");
                options.Scope.Add("email");

                // Save tokens the authentication cookie
                options.SaveTokens = true;
            })
            .AddFacebook(options =>
            {
                options.AppId = configuration["Authentication:Facebook:AppId"]!;
                options.AppSecret = configuration["Authentication:Facebook:AppSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.SaveTokens = true;
            })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = configuration["Authentication:Microsoft:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.SaveTokens = true;
            });

            // Face Recognition Service
            services.AddSingleton<FaceRecognition>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();

                // base dir = bin\Debug\net8.0
                var baseDir = AppContext.BaseDirectory;
                var relative = config["FaceModelsPath"] ?? "Models/FaceModels";
                var modelsPath = Path.Combine(baseDir, relative);

                if (!Directory.Exists(modelsPath))
                {
                    throw new DirectoryNotFoundException($"FaceModelsPath not found: {modelsPath}");
                }

                return FaceRecognition.Create(modelsPath);
            });

            services.AddAuthorization();

            // Merged Services (NO DUPLICATES)
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IWishlistItemService, WishlistItemService>();

            services.AddScoped<IProductImageUrlService, ProductImageUrlService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<ICartService, CartService>();
            
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            
            services.AddScoped<IFaceIdService, FaceIdService>();

            services.AddScoped<IProductReviewService, ProductReviewService>();
            services.AddScoped<IRatingCalculatorService, RatingCalculatorService>();

            return services;
        }
    }
}