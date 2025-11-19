
using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.ModelVM.CartItem;
using Ecom.BLL.ModelVM.Category;
using Ecom.DAL.Entity;

using Microsoft.Data.SqlClient;

using Ecom.BLL.ModelVM.Product;
using Ecom.BLL.ModelVM.ProductReview;

namespace Ecom.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // ----------------------------------------
            // ## Category Mappings
            // ----------------------------------------
            // CreateCategoryVM -> Category 
            CreateMap<AddCategoryVM, Category>()
                .ConstructUsing(vm => new Category(vm.Name!, vm.ImageUrl!, vm.CreatedBy!));
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
            // AddCartVM -> Cart
            CreateMap<AddCartVM, Cart>()
                .ConstructUsing(vm => new Cart(vm.AppUserId!, vm.CreatedBy!));
            // Cart <-> DeleteCartVM
            CreateMap<Cart, DeleteCartVM>().ReverseMap();
            // ----------------------------------------
            // ## End Cart Mappings
            // ----------------------------------------

            // ----------------------------------------
            // ## Cart Item Mappings
            // ----------------------------------------
            // Cart <-> GetCartItemVM
            CreateMap<CartItem, GetCartItemVM>().ReverseMap();
            // Cart <-> UpdateCartItemVM
            CreateMap<CartItem, UpdateCartItemVM>().ReverseMap();
            // AddCartItemVM -> Cart 
            CreateMap<AddCartItemVM, CartItem>()
                .ConstructUsing(vm => new CartItem(vm.ProductId, 
                                                   vm.CartId, 
                                                   vm.Quantity, 
                                                   vm.UnitPrice, 
                                                   vm.CreatedBy));

            // Cart <-> DeleteCartItemVM
            CreateMap<CartItem, DeleteCartItemVM>().ReverseMap();
            // ----------------------------------------
            // ## End Cart Item Mappings
            // ----------------------------------------




            CreateMap<ProductImageUrl, GetProductImageUrlVM>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : null));

            CreateMap<CreateProductImageUrlVM, ProductImageUrl>()
                .ConstructUsing(vm => new ProductImageUrl(vm.ImageUrl!, vm.ProductId, vm.CreatedBy!));

            CreateMap<UpdateProductImageUrlVM, ProductImageUrl>()
                .ConstructUsing(vm => new ProductImageUrl(vm.ImageUrl!, vm.ProductId, vm.UpdatedBy!))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ProductImageUrl, DeleteProductImageUrlVM>().ReverseMap();

            //Product Mapping
            // Product mappings
            CreateMap<Product, GetProductVM>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<CreateProductVM, Product>()
                .ConstructUsing(vm => new Product(
                    vm.Title, vm.Description, vm.Price, vm.DiscountPercentage,
                    vm.Stock, vm.ThumbnailUrl ?? "default.png", vm.CreatedBy ?? "system", vm.BrandId, vm.CategoryId
                ));

            CreateMap<Product, UpdateProductVM>().ReverseMap(); // Update uses Update() inside repo

            CreateMap<Product, DeleteProductVM>().ReverseMap();


            //ProductReviewMapping
            // Entity -> GetVM
            CreateMap<CreateProductVM, Product>()
           .ForMember(dest => dest.ThumbnailUrl, opt => opt.Ignore()) // file handling outside
           .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
           .ForMember(dest => dest.ProductImageUrls, opt => opt.Ignore())
           .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            // UPDATE
            CreateMap<UpdateProductVM, Product>()
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.Ignore())  // you will set manually
                .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImageUrls, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())        // rating updated separately
                .ForMember(dest => dest.QuantitySold, opt => opt.Ignore()).ReverseMap();

            // GET
            CreateMap<Product, GetProductVM>()
                .ForMember(dest => dest.ThumbnailUrl,
                    opt => opt.MapFrom(src => src.ThumbnailUrl))
                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();
            //AddQuantitySoldVM

            CreateMap<Product, AddQuantitySoldVM>().ReverseMap();
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

            // Role Mappings
            CreateMap<IdentityRole, RoleVM>().ReverseMap();


        }

    }
}
