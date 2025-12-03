using AutoMapper;
using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.ModelVM.CartItem;
using Ecom.BLL.ModelVM.Category;
using Ecom.BLL.ModelVM.FaceId;
using Ecom.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Ecom.BLL.ModelVM.Product;
using Ecom.BLL.ModelVM.ProductReview;
using Ecom.BLL.ModelVM.Order;
using Ecom.BLL.ModelVM.OrderItem;

namespace Ecom.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // ----------------------------------------
            // ## Category Mappings
            // ----------------------------------------
            CreateMap<AddCategoryVM, Category>()
                .ConstructUsing(vm => new Category(vm.Name!, vm.ImageUrl!, vm.CreatedBy!));

            CreateMap<Category, UpdateCategoryVM>().ReverseMap();
            CreateMap<Category, GetCategoryVM>().ReverseMap();
            CreateMap<Category, DeleteCategoryVM>().ReverseMap();
            // ----------------------------------------

            // ----------------------------------------
            // ## Cart Mappings
            // ----------------------------------------
            CreateMap<Cart, GetCartVM>().ReverseMap()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<Cart, UpdateCartVM>().ReverseMap();

            CreateMap<AddCartVM, Cart>()
                .ConstructUsing(vm => new Cart(vm.AppUserId!, vm.CreatedBy!));

            CreateMap<Cart, DeleteCartVM>().ReverseMap();
            // ----------------------------------------

            // ----------------------------------------
            // ## Cart Item Mappings
            // ----------------------------------------
            CreateMap<CartItem, GetCartItemVM>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Title));
            CreateMap<CartItem, UpdateCartItemVM>().ReverseMap();

            CreateMap<AddCartItemVM, CartItem>()
                .ConstructUsing(vm => new CartItem(vm.ProductId,
                                                   vm.CartId,
                                                   vm.Quantity,
                                                   vm.UnitPrice,
                                                   vm.CreatedBy));

            CreateMap<CartItem, DeleteCartItemVM>().ReverseMap();
            // ----------------------------------------

            // ----------------------------------------
            // ## Product Image URL Mappings
            // ----------------------------------------
            CreateMap<ProductImageUrl, GetProductImageUrlVM>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Title : null));

            CreateMap<CreateProductImageUrlVM, ProductImageUrl>()
                .ConstructUsing(vm => new ProductImageUrl(vm.ImageUrl!, vm.ProductId, vm.CreatedBy!));

            CreateMap<UpdateProductImageUrlVM, ProductImageUrl>()
                .ConstructUsing(vm => new ProductImageUrl(vm.ImageUrl!, vm.ProductId, vm.UpdatedBy!))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ProductImageUrl, DeleteProductImageUrlVM>().ReverseMap();
            // ----------------------------------------

            // ----------------------------------------
            // ## Product Mappings
            // ----------------------------------------

            CreateMap<CreateProductVM, Product>()
                .ConstructUsing(vm => new Product(
                    vm.Title, vm.Description, vm.Price, vm.DiscountPercentage,
                    vm.Stock, vm.ThumbnailUrl ?? "default.png", vm.CreatedBy ?? "system", vm.BrandId, vm.CategoryId
                ));

            CreateMap<Product, UpdateProductVM>().ReverseMap();
            CreateMap<Product, DeleteProductVM>().ReverseMap();

            // FIX duplicate CreateProductVM mapping (kept only one)
            CreateMap<UpdateProductVM, Product>().ReverseMap();

            CreateMap<Product, GetProductVM>()
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.ProductReviews))
                .ReverseMap();

            CreateMap<Product, AddQuantitySoldVM>().ReverseMap();

            // ---------------------------------------------------------
            // 1) ProductReviewCreateVM → ProductReview (using constructor)
            // ---------------------------------------------------------
            CreateMap<ProductReviewCreateVM, ProductReview>()
                .ConstructUsing(src =>
                    new ProductReview(
                        src.Title,
                        src.Description,
                        src.Rating,
                        src.CreatedBy ?? "system",
                        src.ProductId,
                        src.AppUserId
                    )
                );

            // ---------------------------------------------------------
            // 2) ProductReview → ProductReviewGetVM
            // ---------------------------------------------------------
            CreateMap<ProductReview, ProductReviewGetVM>()
                .ForMember(dest => dest.ProductTitle,
                    opt => opt.MapFrom(src => src.Product.Title))

                .ForMember(dest => dest.AppUserDisplayName,
                    opt => opt.MapFrom(src => src.AppUser.DisplayName))  // adjust field if different

                // copy all basic fields normally
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId));

            // ---------------------------------------------------------
            // 3) ProductReviewUpdateVM → ProductReview
            // (used for mapping into existing tracked entity)
            // ---------------------------------------------------------
            CreateMap<ProductReviewUpdateVM, ProductReview>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) =>
                        srcMember != null)); // skip null to avoid overwriting

            // DeletedVM is handled manually → no need for mapper

            // ----------------------------------------
            // ## Brand Mappings
            // ----------------------------------------
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
            // ----------------------------------------

            // ----------------------------------------
            // ## Address Mappings
            // ----------------------------------------
            CreateMap<CreateAddressVM, Address>()
                .ConstructUsing(vm => new Address(
                    vm.Street, vm.City, vm.Country, vm.PostalCode ?? string.Empty, vm.CreatedBy, vm.AppUserId, vm.Latitude, vm.Longitude
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

            CreateMap<Address, GetAddressVM>();
            // ----------------------------------------

            // ----------------------------------------
            // ## WishlistItem Mappings
            // ----------------------------------------
            CreateMap<CreateWishlistItemVM, WishlistItem>()
                .ConstructUsing(vm => new WishlistItem(vm.AppUserId, vm.ProductId, vm.CreatedBy));

            CreateMap<DeleteWishlistItemVM, WishlistItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<WishlistItem, GetWishlistItemVM>()
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Product.ThumbnailUrl));
            // ----------------------------------------

            // ----------------------------------------
            // ## User Mappings
            // ----------------------------------------
            CreateMap<RegisterUserVM, AppUser>()
                .ConstructUsing(vm => new AppUser(vm.Email!,
                                                  vm.DisplayName,
                                                  vm.ProfileImageUrl,
                                                  vm.Email!,
                                                  vm.PhoneNumber));

            CreateMap<AppUser, GetUserVM>();

            CreateMap<UpdateUserVM, AppUser>()
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // ----------------------------------------

            // ----------------------------------------
            // ## Role Mappings
            // ----------------------------------------
            CreateMap<IdentityRole, RoleVM>().ReverseMap();
            // ----------------------------------------

            // ----------------------------------------
            // ## Order Mappings
            // ----------------------------------------
            CreateMap<Order, GetOrderVM>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, GetOrderItemVM>();

            CreateMap<CreateOrderVM, Order>();

            CreateMap<CreateOrderItemVM, OrderItem>();

            CreateMap<UpdateOrderVM, Order>()
                .ForAllMembers(opt => opt.Ignore());

            // For mapping GetCartItemVM to OrderItem (converting cart to order)
            CreateMap<GetCartItemVM, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
            // ----------------------------------------

            // ----------------------------------------
            // ## Payment Mappings
            // ---------------------------------------
            CreateMap<PaymentResultVM, Payment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentId));

            CreateMap<Payment, GetPaymentVM>().ReverseMap();
            // ----------------------------------------

            // ----------------------------------------
            // ## FaceId Mappings
            // ----------------------------------------
            CreateMap<RegisterFaceIdVM, FaceId>()
                    .ForMember(dest => dest.Encoding,
                        opt => opt.MapFrom(src => FaceId.DoubleArrayToBytes(src.Encoding)))
                    .ForMember(dest => dest.AppUserId,
                        opt => opt.MapFrom(src => src.AppUserId))
                    .ForMember(dest => dest.CreatedBy,
                        opt => opt.MapFrom(src => src.CreatedBy));

            CreateMap<UpdateFaceIdVM, FaceId>()
                .ForMember(dest => dest.Encoding,
                    opt => opt.MapFrom(src => FaceId.DoubleArrayToBytes(src.Encoding)))
                .ForMember(dest => dest.AppUserId,
                    opt => opt.MapFrom(src => src.AppUserId))
                .ForMember(dest => dest.UpdatedBy,
                    opt => opt.MapFrom(src => src.UpdatedBy));
            // ----------------------------------------
        }
    }
}