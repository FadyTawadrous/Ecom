using Ecom.BLL.ModelVM.WishlistItem;
using Ecom.BLL.Service.Abstraction;
using Ecom.DAL.Entity;
using Ecom.DAL.Repo.Abstraction;
using System.Linq.Expressions;

namespace Ecom.BLL.Service.Implementation
{
    public class WishlistItemService : IWishlistItemService
    {
        private readonly IWishlistItemRepo _wishlistItemRepo;
        private readonly IMapper _mapper;

        public WishlistItemService(IWishlistItemRepo wishlistItemRepo, IMapper mapper)
        {
            _wishlistItemRepo = wishlistItemRepo;
            _mapper = mapper;
        }

        // Get
        public async Task<ResponseResult<GetWishlistItemVM>> GetByIdAsync(int id)
        {
            try
            {
                var wishlistItem = await _wishlistItemRepo.GetByIdAsync(id, 
                    includes: [w => w.AppUser, w => w.Product]);

                if (wishlistItem == null)
                    return new ResponseResult<GetWishlistItemVM>(null, 
                        $"Wishlist item with ID {id} not found.", false);

                var mappedWishlistItem = _mapper.Map<GetWishlistItemVM>(wishlistItem);
                return new ResponseResult<GetWishlistItemVM>(mappedWishlistItem, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetWishlistItemVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetWishlistItemVM>>> GetAllAsync(
            string? searchName = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Define filter expression
                Expression<Func<WishlistItem, bool>> filter = w =>
                    !w.IsDeleted &&
                    (string.IsNullOrEmpty(searchName) ||
                     w.Product.Title.ToLower().Contains(searchName.ToLower()));

                var items = await _wishlistItemRepo.GetAllAsync(
                    filter: filter,
                    pageSize: pageSize,
                    pageNumber: pageNumber,
                    includes: [w => w.AppUser, w => w.Product]);

                if (items == null || !items.Any())
                    return new ResponseResult<IEnumerable<GetWishlistItemVM>>(null,
                        "No wishlist items found.", false);

                var mappedItems = _mapper.Map<IEnumerable<GetWishlistItemVM>>(items);
                return new ResponseResult<IEnumerable<GetWishlistItemVM>>(mappedItems, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetWishlistItemVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetWishlistItemVM>>> GetAllByUserIdAsync(string userId,
            string? searchName = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Define filter expression
                Expression<Func<WishlistItem, bool>> filter = w =>
                    !w.IsDeleted &&
                    (string.IsNullOrEmpty(searchName) ||
                     w.Product.Title.ToLower().Contains(searchName.ToLower()));

                var items = await _wishlistItemRepo.GetAllByUserIdAsync(
                    userId: userId,
                    filter: filter,
                    pageSize: pageSize,
                    pageNumber: pageNumber,
                    includes: w => w.Product);

                if (items == null || !items.Any())
                    return new ResponseResult<IEnumerable<GetWishlistItemVM>>(null, 
                        "No wishlist items found.", false);

                var mappedItems = _mapper.Map<IEnumerable<GetWishlistItemVM>>(items);
                return new ResponseResult<IEnumerable<GetWishlistItemVM>>(mappedItems, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetWishlistItemVM>>(null, ex.Message, false);
            }
        }

        // Create
        public async Task<ResponseResult<bool>> CreateAsync(CreateWishlistItemVM model)
        {
            try
            {
                // Map VM -> Entity
                var newItem = _mapper.Map<WishlistItem>(model);

                // Call the repo to add the new item
                var result = await _wishlistItemRepo.AddAsync(newItem);

                // Return the response
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }
                return new ResponseResult<bool>(false, "Failed to save wishlist item to the database.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        // Delete
        public async Task<ResponseResult<bool>> DeleteAsync(DeleteWishlistItemVM model)
        {
            try
            {
                // Get the tracked entity
                var itemToDelete = await _wishlistItemRepo.GetByIdAsync(model.Id);
                if (itemToDelete == null)
                {
                    return new ResponseResult<bool>(false, "Wishlist item not found.", false);
                }

                // Delete the employee using the repo
                bool result = await _wishlistItemRepo.ToggleDeleteStatusAsync(model.Id, model.DeletedBy); // Soft delete
                //bool result = await _wishlistItemRepo.DeleteAsync(model.Id); // Hard delete

                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }

                return new ResponseResult<bool>(false, "Failed to toggle delete status.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        // Add/Move to cart
        public async Task<ResponseResult<bool>> AddToCartAsync(int id, string userId)
        {
            try
            {
                // Get wishlist item product details
                var wishlistItem = await _wishlistItemRepo.GetByIdAsync(id, w => w.Product);

                if (wishlistItem == null || wishlistItem.AppUserId != userId)
                    return new ResponseResult<bool>(false, "Wishlist item not found or unauthorized.", false);

                // Call the cart service to add/move the wishlist item to cart using product id
                var result = true;// = await _cartRepo.AddToCartAsync(id, w => w.Product.Id);

                // Return the response
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }
                return new ResponseResult<bool>(false, "Failed to add/move wishlist item to cart.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }
    }
}
