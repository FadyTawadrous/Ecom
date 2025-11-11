
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
                // Uploading Image to the Server
                // If an image is uploaded, get the URL 
                // Else set to default image
                if (model.Image != null)
                {
                    model.ImageUrl = await Upload.UploadFileAsync("File", model.Image);
                }
                else model.ImageUrl = "default-category.png";

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

        public Task<ResponseResult<bool>> DeleteAsync(DeleteCategoryVM model)
        {
            throw new NotImplementedException();
        }

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

        public Task<ResponseResult<bool>> ToggleDeleteAsync(int id, string userModified)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<bool>> UpdateAsync(UpdateCategoryVM model)
        {
            throw new NotImplementedException();
        }
    }
}
