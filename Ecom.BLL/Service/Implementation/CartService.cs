
using Ecom.BLL.ModelVM.Cart;
using Ecom.BLL.ModelVM.Category;
using Ecom.DAL.Entity;

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

        // Delete Cart (Soft Delete)
        public async Task<ResponseResult<bool>> DeleteAsync(DeleteCartVM model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return new ResponseResult<bool>(false, "Invalid Id", false);
                }

                // Toggling the IsDeleted status of the cart
                bool isDeleted = await _cartRepo.ToggleDeleteAsync(model.Id, model.DeletedBy);
                if (isDeleted)
                {
                    return new ResponseResult<bool>(true, "Cart deleted successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to delete cart", false);
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
                    var cart = await _cartRepo.GetByIdAsync(id);

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

        // Hard Delete Cart
        public async Task<ResponseResult<bool>> HardDeleteAsync(DeleteCartVM model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return new ResponseResult<bool>(false, "Invalid Id", false);
                }

                // Permanently deleting the cart from database
                bool isDeleted = await _cartRepo.HardDeleteAsync(model.Id);
                if (isDeleted)
                {
                    return new ResponseResult<bool>(true, "Cart hard deleted successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to hard delete cart", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Update Cart
        public async Task<ResponseResult<bool>> UpdateAsync(UpdateCartVM model)
        {
            try
            {
                // Checking if the cart exists
                var existing = await _cartRepo.GetByIdAsync(model.Id);
                if (existing == null)
                {
                    return new ResponseResult<bool>(false, "Cart not found", false);
                }

                // mapping ViewModel to Entity
                var cart = _mapper.Map<Cart>(model);

                // Updating Cart in Database
                bool isUpdated = await _cartRepo.UpdateAsync(cart);
                if (isUpdated)
                {
                    return new ResponseResult<bool>(true, "Cart updated successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to update cart", false);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
