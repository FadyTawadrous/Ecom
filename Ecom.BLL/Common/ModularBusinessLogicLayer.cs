

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
            //services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IProductReviewService, ProductReviewService>();
            services.AddScoped<IRatingCalculatorService, RatingCalculatorService>();

            return services;
        }
    }
}
