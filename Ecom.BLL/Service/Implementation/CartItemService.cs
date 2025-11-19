
using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.ModelVM.CartItem;
using Ecom.DAL.Entity;

namespace Ecom.BLL.Service.Implementation
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepo _cartItemRepo;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepo cartItemRepo, IMapper mapper)
        {
            _cartItemRepo = cartItemRepo;
            _mapper = mapper;
        }

        // Add CArt Item
        public async Task<ResponseResult<bool>> AddAsync(AddCartItemVM model)
        {
            try
            {
                // Check if product already exists in the cart
                var existing = await _cartItemRepo
                    .GetByCartIdAndProductIDAsync(model.CartId, model.ProductId);

                if (existing != null)
                {
                    // Business rule: Increase quantity
                    existing.Update(
                        existing.Quantity + model.Quantity,
                        model.UnitPrice,
                        model.CreatedBy
                    );

                    await _cartItemRepo.UpdateAsync(existing);

                    return new ResponseResult<bool>(true, "Quantity updated in existing CartItem", true);
                }

                // Otherwise create a new cart item
                var entity = _mapper.Map<CartItem>(model);
                var created = await _cartItemRepo.AddAsync(entity);

                if (!created)
                    return new ResponseResult<bool>(false, "Failed to add new CartItem", false);

                return new ResponseResult<bool>(true, "CartItem created successfully", true);
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
        public async Task<ResponseResult<bool>> HardDeleteAsync(DeleteCartItemVM model)
        {
            try
            {
                // Permanently deleting the cart Item from database
                bool isDeleted = await _cartItemRepo.HardDeleteAsync(model.Id);
                if (isDeleted)
                {
                    return new ResponseResult<bool>(true, "CartItem hard deleted successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to hard delete cartItem", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Soft Delete Cart Item
        public async Task<ResponseResult<bool>> DeleteAsync(DeleteCartItemVM model)
        {
            try
            {
                // Toggling the IsDeleted status of the cart Item
                bool isDeleted = await _cartItemRepo.ToggleDeleteAsync(model.Id, model.DeletedBy);
                if (isDeleted)
                {
                    return new ResponseResult<bool>(true, "CartItem deleted successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to delete cartItem", false);
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
    }
}
