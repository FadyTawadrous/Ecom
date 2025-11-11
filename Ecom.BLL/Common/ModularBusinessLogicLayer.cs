
namespace Ecom.BLL.Common
{
    public static class ModularBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessInBLL(this IServiceCollection services)
        {
            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));


            services.AddScoped<IProductImageUrlService, ProductImageUrlService>();
            services.AddScoped<IBrandService, BrandService>();
            return services;
        }
    }
}
