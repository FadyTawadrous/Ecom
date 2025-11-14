
using Ecom.BLL.Service.Abstraction;
using Ecom.BLL.Service.Implementation;

namespace Ecom.BLL.Common
{
    public static class ModularBusinessLogicLayer
    {
        public static IServiceCollection AddBusinessInBLL(this IServiceCollection services)
        {
            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));


            services.AddScoped<IProductImageUrlService, ProductImageUrlService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IWishlistItemService, WishlistItemService>();
            return services;
        }
    }
}
