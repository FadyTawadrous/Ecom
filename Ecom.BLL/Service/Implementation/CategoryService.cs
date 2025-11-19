
using Ecom.BLL.Helper;
using Ecom.BLL.ModelVM.Category;
using Ecom.BLL.Service.Abstraction;
using Ecom.DAL.Entity;
using Ecom.DAL.Repo.Abstraction;

namespace Ecom.BLL.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        // Add Category 
        public async Task<ResponseResult<bool>> AddAsync(AddCategoryVM model)
        {
            try
            {
                // Validate name
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return new ResponseResult<bool>(false, "Category name cannot be empty", false);
                }

                // Checking if a category with the same name already exists
                if (await _categoryRepo.ExistsByNameAsync(model.Name))
                {
                    return new ResponseResult<bool>(false, "Category with the same name already exists", false);
                }

                // Uploading Image to the Server
                // If an image is uploaded, get the URL 
                // Else set to default image
                if (model.Image != null)
                {
                    model.ImageUrl = await Upload.UploadFileAsync("File/CategoryImages", model.Image);
                }
                else if (string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    model.ImageUrl = "default-category.png";
                }

                // mapping ViewModel to Entity
                var category = _mapper.Map<Category>(model);

                // Adding Category to Database
                var isAdded = await _categoryRepo.AddAsync(category);
                // Returning Response   
                if (isAdded)
                {
                    return new ResponseResult<bool>(true, "Category added successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to add category", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Delete Category (Soft Delete)
        public async Task<ResponseResult<bool>> DeleteAsync(DeleteCategoryVM model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return new ResponseResult<bool>(false, "Invalid Category Id", false);
                }

                // Toggling the IsDeleted status of the category
                bool isDeleted = await _categoryRepo.ToggleDeleteAsync(model.Id, model.DeletedBy);
                if (isDeleted)
                {
                    return new ResponseResult<bool>(true, "Category deletion status toggled successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to toggle category deletion status", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get All Categories
        public async Task<ResponseResult<IEnumerable<GetCategoryVM>>> GetAllAsync()
        {
            try
            {
                // Getting all categories which are not deleted
                var categories = await _categoryRepo.GetAllAsync(c => !c.IsDeleted);

                // Mapping Entity to ViewModel
                var categoryVMs = _mapper.Map<IEnumerable<GetCategoryVM>>(categories);

                // Returning Response
                return new ResponseResult<IEnumerable<GetCategoryVM>>(categoryVMs, "Categories retrieved successfully", true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get Category by Id
        public async Task<ResponseResult<GetCategoryVM>> GetByIdAsync(int id)
        {
            try
            {
                if (id > 0) 
                { 
                // Getting category by id
                var category = await _categoryRepo.GetByIdAsync(id);

                    // Checking if category exists and is not deleted
                    if (category == null || category.IsDeleted)
                    {
                        return new ResponseResult<GetCategoryVM>(null!, "Category not found", false);
                    }

                    // Mapping Entity to ViewModel
                    var categoryVM = _mapper.Map<GetCategoryVM>(category);

                    // Returning Response
                    return new ResponseResult<GetCategoryVM>(categoryVM, "Category retrieved successfully", true);
                }
                return new ResponseResult<GetCategoryVM>(null!, "Invalid Id", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Hard Delete Category
        public async Task<ResponseResult<bool>> HardDeleteAsync(DeleteCategoryVM model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return new ResponseResult<bool>(false, "Invalid Category Id", false);
                }

                // Permanently deleting the category from database
                bool isDeleted = await _categoryRepo.HardDeleteAsync(model.Id);
                if (isDeleted)
                {
                    return new ResponseResult<bool>(true, "Category hard deleted successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to hard delete category", false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<ResponseResult<bool>> ToggleDeleteAsync(int id, string userModified)
        {
            throw new NotImplementedException();
        }

        // Update Category
        public async Task<ResponseResult<bool>> UpdateAsync(UpdateCategoryVM model)
        {
            try
            {
                // Validate Id
                if (model.Id <= 0)
                {
                    return new ResponseResult<bool>(false, "Invalid Category Id", false);
                }

                // Checking if the category exists
                var existing = await _categoryRepo.GetByIdAsync(model.Id);
                if (existing == null)
                {
                    return new ResponseResult<bool>(false, "Category not found", false);
                }

                // Uploading Image to the Server
                // If an image is uploaded, get the URL 
                // Else keep the existing image URL
                if (model.Image != null)
                {
                    model.ImageUrl = await Upload.UploadFileAsync("File/CategoryImages", model.Image);
                }

                // mapping ViewModel to Entity
                var category = _mapper.Map<Category>(model);

                // Updating Category in Database
                bool isUpdated = await _categoryRepo.UpdateAsync(category);
                if (isUpdated)
                {
                    return new ResponseResult<bool>(true, "Category updated successfully", true);
                }
                return new ResponseResult<bool>(false, "Failed to update category", false);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
