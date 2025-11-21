
namespace Ecom.BLL.Service.Implementation
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepo _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepo brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        public async Task<ResponseResult<IEnumerable<GetBrandVM>>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var brands = await _brandRepo.GetAllAsync(b => includeDeleted || !b.IsDeleted);
                var result = _mapper.Map<IEnumerable<GetBrandVM>>(brands);
                return new ResponseResult<IEnumerable<GetBrandVM>>(result, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetBrandVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<GetBrandVM>> GetByIdAsync(int id)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(id);
                if (brand == null)
                    return new ResponseResult<GetBrandVM>(null, "Brand not found.", false);

                var vm = _mapper.Map<GetBrandVM>(brand);
                return new ResponseResult<GetBrandVM>(vm, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetBrandVM>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> CreateAsync(CreateBrandVM model)
        {
            try
            {
                string? uploadedImageUrl = "default.png";
                if (model.Image != null)
                {
                    try
                    {
                        uploadedImageUrl = await Upload.UploadFileAsync("Images/BrandImages", model.Image);
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<bool>(false, $"File upload failed: {ex.Message}", false);
                    }
                }
                model.ImageUrl = uploadedImageUrl;
                var brand = new Brand(model.Name, model.ImageUrl, model.CreatedBy);
                await _brandRepo.AddAsync(brand);
                await _brandRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> UpdateAsync(UpdateBrandVM model)
        {
            try
            {
                //Find Tracked Brand From Repo
                var oldbrand = await _brandRepo.GetByIdAsync(model.Id);
                if (oldbrand == null)
                    return new ResponseResult<bool>(false, "Brand not found.", false);

                //handle Photo Upload
                string? uploadedImageUrl = oldbrand.ImageUrl;
                if (model.Image != null)
                {
                    try
                    {
                        uploadedImageUrl = await Upload.UploadFileAsync("Images/BrandImages", model.Image); // Upload image to server
                        if (!string.IsNullOrEmpty(oldbrand.ImageUrl))
                        {
                            await Upload.RemoveFileAsync("Images", oldbrand.ImageUrl); // Remove old image if exists
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<bool>(false, $"File update failed: {ex.Message}", false);
                    }
                }
                model.ImageUrl = uploadedImageUrl;

                if (!oldbrand.Update(model.Name, model.ImageUrl, model.UpdatedBy))
                    return new ResponseResult<bool>(false, "Invalid update data.", false);
                //apply changes
                await _brandRepo.UpdateAsync(oldbrand);
                await _brandRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<bool>> DeleteAsync(DeleteBrandVM model)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(model.Id);
                if (brand == null)
                    return new ResponseResult<bool>(false, "Brand not found.", false);

                await _brandRepo.DeleteAsync(model.Id, model.DeletedBy);
                await _brandRepo.SaveChangesAsync();
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<DeleteBrandVM>> GetDeleteModelAsync(int id)
        {
            try
            {
                var brand = await _brandRepo.GetByIdAsync(id);
                if (brand == null)
                    return new ResponseResult<DeleteBrandVM>(null, "Brand not found.", false);

                var vm = new DeleteBrandVM
                {
                    Id = brand.Id,
                    DeletedBy = brand.DeletedBy ?? "System"
                };

                return new ResponseResult<DeleteBrandVM>(vm, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<DeleteBrandVM>(null, ex.Message, false);
            }
        }
    }

}
