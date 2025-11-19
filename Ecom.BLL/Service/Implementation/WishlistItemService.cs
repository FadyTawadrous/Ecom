
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
                     string.IsNullOrEmpty(searchName) ||
                     w.Product.Title.ToLower().Contains(searchName.ToLower());

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
                     string.IsNullOrEmpty(searchName) ||
                     w.Product.Title.ToLower().Contains(searchName.ToLower());

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

                // Delete the item using the repo (hard delete)
                bool result = await _wishlistItemRepo.DeleteAsync(model.Id);
                if (result)
                {
                    return new ResponseResult<bool>(true, null, true);
                }

                return new ResponseResult<bool>(false, "Failed to delete wishlist item.", false);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        // Get Delete Model
        public async Task<ResponseResult<DeleteWishlistItemVM>> GetDeleteModelAsync(int id)
        {
            try
            {
                var item = await _wishlistItemRepo.GetByIdAsync(id);
                if (item == null)
                    return new ResponseResult<DeleteWishlistItemVM>(null,
                        $"Wishlist item with ID {id} not found.", false);

                var mappedItem = _mapper.Map<DeleteWishlistItemVM>(item);
                return new ResponseResult<DeleteWishlistItemVM>(mappedItem, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<DeleteWishlistItemVM>(null, ex.Message, false);
            }
        }
    }
}
