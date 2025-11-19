
using Ecom.BLL.ModelVM.Address;
using Ecom.BLL.ModelVM.Brand;
using Ecom.BLL.ModelVM.WishlistItem;
using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.ModelVM.Category;
using Ecom.DAL.Entity;

using Microsoft.Data.SqlClient;

namespace Ecom.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // ----------------------------------------
            // ## Category Mappings
            // ----------------------------------------
            // Category <-> CreateCategoryVM
            CreateMap<Category, AddCategoryVM>().ReverseMap();
            // Category <-> UpdateCategoryVM
            CreateMap<Category, UpdateCategoryVM>().ReverseMap();
            // Category <-> GetCategoryVM
            CreateMap<Category, GetCategoryVM>().ReverseMap()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            // Category <-> DeleteCategoryVM
            CreateMap<Category, DeleteCategoryVM>().ReverseMap();
            // ----------------------------------------
            // ## End Category Mappings
            // ----------------------------------------

            // ----------------------------------------
            // ## Cart Mappings
            // ----------------------------------------
            // Cart <-> GetCartVM
            CreateMap<Cart, GetCartVM>().ReverseMap()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
            // Cart <-> UpdateCartVM
            CreateMap<Cart, UpdateCartVM>().ReverseMap();
            // Cart <-> AddCartVM
            CreateMap<Cart, AddCartVM>().ReverseMap();
            // Cart <-> DeleteCartVM
            CreateMap<Cart, DeleteCartVM>().ReverseMap();
            // ----------------------------------------
            // ## End Cart Mappings
            // ----------------------------------------




            CreateMap<ProductImageUrl, GetProductImageUrlVM>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : null));

            CreateMap<CreateProductImageUrlVM, ProductImageUrl>()
                .ConstructUsing(vm => new ProductImageUrl(vm.ImageUrl!, vm.ProductId, vm.CreatedBy!));

            CreateMap<UpdateProductImageUrlVM, ProductImageUrl>()
                .ConstructUsing(vm => new ProductImageUrl(vm.ImageUrl!, vm.ProductId, vm.UpdatedBy!))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ProductImageUrl, DeleteProductImageUrlVM>().ReverseMap();

            //Brand Mappings
           
            CreateMap<Brand, GetBrandVM>().ReverseMap();

            CreateMap<CreateBrandVM, Brand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateBrandVM, Brand>()
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<DeleteBrandVM, Brand>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ReverseMap();

            // Address Mappings
            CreateMap<CreateAddressVM, Address>()
                .ConstructUsing(vm => new Address(
                    vm.Street, vm.City, vm.Country, vm.PostalCode ?? string.Empty, vm.CreatedBy, vm.AppUserId
                ));

            CreateMap<UpdateAddressVM, Address>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ReverseMap();

            CreateMap<DeleteAddressVM, Address>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ReverseMap();

            CreateMap<Address, GetAddressVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode));

            // WishlistItem Mappings
            CreateMap<CreateWishlistItemVM, WishlistItem>()
                .ConstructUsing(vm => new WishlistItem(
                    vm.AppUserId, vm.ProductId, vm.CreatedBy
                ));

            CreateMap<DeleteWishlistItemVM, WishlistItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<WishlistItem, GetWishlistItemVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Product.ThumbnailUrl));


            // User Mappings
            // Maps from the RegisterUserVM to the AppUser entity
            CreateMap<RegisterUserVM, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

            // Maps from the AppUser entity to the GetUserVM
            CreateMap<AppUser, GetUserVM>();

            // 3. Map for Updating (Special Case)
            // This tells AutoMapper how to apply an UpdateDto *onto* an
            // existing AppUser object, ignoring any null values from the DTO.
            CreateMap<UpdateUserVM, AppUser>()
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                

        }
    }
}
