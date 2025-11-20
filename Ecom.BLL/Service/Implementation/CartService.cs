
namespace Ecom.BLL.Service.Implementation
{
    // Implementation of Cart Service
    public class CartService : ICartService
    {
        // Dependency Injection of Cart Repository and AutoMapper
        private readonly ICartRepo _cartRepo;
        private readonly IMapper _mapper;
        public CartService(ICartRepo cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        // Add Cart
        public async Task<ResponseResult<bool>> AddAsync(AddCartVM model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.AppUserId))
                {
                    return new ResponseResult<bool>(false, "Invalid UserId", false);
                }
            

                // mapping ViewModel to Entity
                var cart = _mapper.Map<Cart>(model);

                // Adding Cart to Database
                bool isAdded = await _cartRepo.AddAsync(cart);
                if (isAdded)
                {
                    return new ResponseResult<bool>(true, "Cart added successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to add cart", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get Cart by User ID
        public async Task<ResponseResult<GetCartVM>> GetByUserIdAsync(string UserId)
        {
            try
            {
                // Validating the user ID 
                if (!string.IsNullOrEmpty(UserId))
                {
                    // Getting cart by user id
                    var cart = await _cartRepo.GetByUserIdAsync(UserId, c => c.CartItems!);

                    // Checking if cart exists   
                    if (cart == null)
                    {
                        return new ResponseResult<GetCartVM>(null!, "cart not found", false);
                    }
                    // Checking if cart is deleted
                    if (cart.IsDeleted)
                    {
                        return new ResponseResult<GetCartVM>(null!, "cart is deleted", false);
                    }

                    // Mapping Entity to ViewModel
                    var cartVM = _mapper.Map<GetCartVM>(cart);

                    // Returning Response  
                    return new ResponseResult<GetCartVM>(cartVM, "Cart retrieved successfully", true);
                }
                return new ResponseResult<GetCartVM>(null!, "Invalid Id", false);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        // Get Cart by ID
        public async Task<ResponseResult<GetCartVM>> GetByIdAsync(int id)
        {
            try
            {
                if (id > 0)
                {
                    // Getting cart by id
                    var cart = await _cartRepo.GetByIdAsync(id, c => c.CartItems!);

                    // Checking if cart exists and is not deleted
                    if (cart == null || cart.IsDeleted)
                    {
                        return new ResponseResult<GetCartVM>(null!, "Cart not found", false);
                    }

                    // Mapping Entity to ViewModel
                    var cartVM = _mapper.Map<GetCartVM>(cart);

                    // Returning Response
                    return new ResponseResult<GetCartVM>(cartVM, "Cart retrieved successfully", true);
                }
                return new ResponseResult<GetCartVM>(null!, "Invalid Id", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Clear Cart by ID
        public async Task<ResponseResult<bool>> ClearCartAsync(int cartId)
        {
            try
            {
                if(cartId > 0)
                {
                    // Clearing cart
                    bool isCleared = await _cartRepo.ClearCartAsync(cartId);
                    if (isCleared)
                    {
                        return new ResponseResult<bool>(true, "Cart cleared successfully", true);
                    }
                    return new ResponseResult<bool>(false, "Failed to clear cart", false);
                }
                return new ResponseResult<bool>(false, "Invalid Cart Id", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
