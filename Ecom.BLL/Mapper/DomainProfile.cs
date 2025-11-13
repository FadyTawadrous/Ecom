
using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.ModelVM.Category;
using Ecom.DAL.Entity;

namespace Ecom.BLL.AutoMapper
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
        
        }

    }
}
