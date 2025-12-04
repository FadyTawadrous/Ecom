
namespace Ecom.BLL.Service.Implementation
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepo _cartItemRepo;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepo cartItemRepo, IMapper mapper, IProductService productService)
        {
            _cartItemRepo = cartItemRepo;
            _mapper = mapper;
            _productService = productService;
        }

        // Add CArt Item
        public async Task<ResponseResult<bool>> AddAsync(AddCartItemVM model)
        {
            try
            {
                // 1- Check if product already exists in the cart
                var existing = await _cartItemRepo
                    .GetByCartIdAndProductIDAsync(model.CartId, model.ProductId);

                // 2-  If exists, update the quantity
                if (existing != null)
                {
                    existing.Update(
                        existing.Quantity + model.Quantity,
                        model.UnitPrice,
                        model.CreatedBy
                    );

                    var isUpdated = await _cartItemRepo.UpdateAsync(existing);
                    if (isUpdated)
                    {
                        // Decrease stock accordingly
                        var result = await _productService.DecreaseStockAsync(model.ProductId, model.Quantity);
                        if (result != null)
                            return new ResponseResult<bool>(true, "Quantity updated in existing CartItem and stock decreased", true);
                        else
                            return new ResponseResult<bool>(false, "Quantity updated in existing CartItem but failed to decrease stock", false);
                    }
                }


                // 3- If not exists, create a new cart item
                var cartItem = _mapper.Map<CartItem>(model);
                var created = await _cartItemRepo.AddAsync(cartItem);

                if (created)
                {
                    return new ResponseResult<bool>(true, "CartItem created successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to add new CartItem", false);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        // Get Cart Item by CartId and ProductId
        public async Task<ResponseResult<GetCartItemVM>> GetByCartIdAndProductIdAsync(int cartId, int productId)
        {
            try
            {
                if (cartId > 0 && productId > 0) 
                {
                    var cartItem = await _cartItemRepo.GetByCartIdAndProductIDAsync(cartId, productId);
                    if (cartItem == null)
                    {
                        return new ResponseResult<GetCartItemVM>(null, "CartItem not found", false);
                    }

                    var cartItemVM = _mapper.Map<GetCartItemVM>(cartItem);

                    return new ResponseResult<GetCartItemVM>(cartItemVM, "CartItem retrieved successfully", true);
                }
                return new ResponseResult<GetCartItemVM>(null , "Invalid Id", false);            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get Cart Item By CartId
        public async Task<ResponseResult<IEnumerable<GetCartItemVM>>> GetByCartIdAsync(int cartId)
        {
            try
            {
                if (cartId > 0)
                {
                    var cartItems = await _cartItemRepo.GetByCartIdAsync(cartId , c => c.Product, c => c.Cart);
                    if (cartItems == null)
                    {
                        return new ResponseResult<IEnumerable<GetCartItemVM>>(null, "CartItems not found", false);
                    }

                    var cartItemsVM = _mapper.Map<IEnumerable<GetCartItemVM>>(cartItems);

                    return new ResponseResult<IEnumerable<GetCartItemVM>>(cartItemsVM, "CartItems retrieved successfully", true);
                }
                return new ResponseResult<IEnumerable<GetCartItemVM>>(null, "Invalid Id", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get Cart Item By Id
        public async Task<ResponseResult<GetCartItemVM>> GetByIdAsync(int id)
        {
            try
            {
                if (id > 0)
                {
                    var cartItem = await _cartItemRepo.GetByIdAsync(id , c => c.Product, c => c.Cart);
                    if (cartItem == null)
                    {
                        return new ResponseResult<GetCartItemVM>(null, "CartItem not found", false);
                    }

                    var cartItemVM = _mapper.Map<GetCartItemVM>(cartItem);

                    return new ResponseResult<GetCartItemVM>(cartItemVM, "CartItem retrieved successfully", true);
                }
                return new ResponseResult<GetCartItemVM>(null, "Invalid Id", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Hard Delete Cart Item
        public async Task<ResponseResult<bool>> DeleteAsync(int id)
        {
            try
            {
                // Checking if the cart item exists
                var model = await _cartItemRepo.GetByIdAsync(id, c => c.Product, c => c.Cart);
                if (model == null)
                {
                    return new ResponseResult<bool>(false, "CartItem not found", false);
                }

                // Deleting Cart Item from Database
                bool isDeleted = await _cartItemRepo.DeleteAsync(id);

                // If deleted, increase the stock accordingly
                if (isDeleted)
                {
                    var result = await _productService.IncreaseStockAsync(model.ProductId, model.Quantity);
                    if(result != null)
                    {
                        return new ResponseResult<bool>(true, "CartItem hard deleted successfully and stock increased", true);
                    }
                    return new ResponseResult<bool>(true, "CartItem hard deleted successfully but failed to increase stock", true);
                }
                return new ResponseResult<bool>(false, "Failed to hard delete cartItem", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Update Cart Item
        public async Task<ResponseResult<bool>> UpdateAsync(UpdateCartItemVM model)
        {
            try
            {
                // Checking if the cart item exists
                var existing = await _cartItemRepo.GetByIdAsync(model.Id);
                if (existing == null)
                {
                    return new ResponseResult<bool>(false, "CartItem not found", false);
                }

                // mapping ViewModel to Entity
                var cart = _mapper.Map<CartItem>(model);

                // Updating Cart Item in Database
                bool isUpdated = await _cartItemRepo.UpdateAsync(cart);
                if (isUpdated)
                {
                    return new ResponseResult<bool>(true, "CartItem updated successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to update cartItem", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseResult<IEnumerable<GetCartItemVM>>> GetByUserIdAsync(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var cartItems = await _cartItemRepo.GetByUserIDAsync(userId, c => c.Product, c => c.Cart);
                    if (cartItems == null)
                    {
                        return new ResponseResult<IEnumerable<GetCartItemVM>>(null, "CartItems not found", false);
                    }
                    var cartItemsVM = _mapper.Map<IEnumerable<GetCartItemVM>>(cartItems);
                    return new ResponseResult<IEnumerable<GetCartItemVM>>(cartItemsVM, "CartItems retrieved successfully", true);
                }
                return new ResponseResult<IEnumerable<GetCartItemVM>>(null, "Invalid UserId", false);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
