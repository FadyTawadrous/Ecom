

using Ecom.BLL.Helper;
using Ecom.BLL.Service.Abstraction;
using Ecom.DAL.Entity;
using Ecom.DAL.Repo.Abstraction;

namespace Ecom.BLL.Service.Implementation

  

   
    {
        public class ProductImageUrlService : IProductImageUrlService
        {
            private readonly IProductImageUrlRepo _imageRepo;
            private readonly IMapper _mapper;

            public ProductImageUrlService(IProductImageUrlRepo imageRepo, IMapper mapper)
            {
                _imageRepo = imageRepo;
                _mapper = mapper;
            }

            // 1️⃣ Get all images
            public async Task<ResponseResult<IEnumerable<GetProductImageUrlVM>>> GetAllAsync()
            {
                try
                {
                    var images = await _imageRepo.GetAllAsync(i => !i.IsDeleted, includes: i => i.Product);
                    var mapped = _mapper.Map<IEnumerable<GetProductImageUrlVM>>(images);
                    return new ResponseResult<IEnumerable<GetProductImageUrlVM>>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<IEnumerable<GetProductImageUrlVM>>(null, ex.Message, false);
                }
            }

            // 2️⃣ Get by ID
            public async Task<ResponseResult<GetProductImageUrlVM>> GetByIdAsync(int id)
            {
                try
                {
                    var image = await _imageRepo.GetByIdAsync(id);
                    if (image == null || image.IsDeleted)
                        return new ResponseResult<GetProductImageUrlVM>(null, "Image not found", false);

                    var mapped = _mapper.Map<GetProductImageUrlVM>(image);
                    return new ResponseResult<GetProductImageUrlVM>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<GetProductImageUrlVM>(null, ex.Message, false);
                }
            }

            // 3️⃣ Create new image
            public async Task<ResponseResult<bool>> CreateAsync(CreateProductImageUrlVM model)
            {
                try
                {
                    // handle file upload (optional)
                    string? uploadedImageUrl = "default.png";
                    if (model.Image != null)
                    {
                        try
                        {
                            uploadedImageUrl = await Upload.UploadFileAsync("ProductImages", model.Image);
                        }
                        catch (Exception ex)
                        {
                            return new ResponseResult<bool>(false, $"File upload failed: {ex.Message}", false);
                        }
                    }

                    model.ImageUrl = uploadedImageUrl;

                    // map VM -> entity
                    var entity = _mapper.Map<ProductImageUrl>(model);

                    // add to DB
                    var result = await _imageRepo.AddAsync(entity);

                    return new ResponseResult<bool>(result, result ? null : "Failed to add image.", result);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<bool>(false, ex.Message, false);
                }
            }

            // 4️⃣ Update existing image
            public async Task<ResponseResult<bool>> UpdateAsync(UpdateProductImageUrlVM model)
            {
                try
                {
                    var existing = await _imageRepo.GetByIdAsync(model.Id);
                    if (existing == null)
                        return new ResponseResult<bool>(false, "Image not found", false);

                    // handle file replacement
                    string? newImageUrl = existing.ImageUrl;
                    if (model.Image != null)
                    {
                        try
                        {
                            newImageUrl = await Upload.UploadFileAsync("ProductImages", model.Image);
                            if (!string.IsNullOrEmpty(existing.ImageUrl))
                            {
                                await Upload.RemoveFileAsync("ProductImages", existing.ImageUrl);
                            }
                        }
                        catch (Exception ex)
                        {
                            return new ResponseResult<bool>(false, $"File update failed: {ex.Message}", false);
                        }
                    }

                    model.ImageUrl = newImageUrl;
                    var entity = _mapper.Map<ProductImageUrl>(model);

                    var result = await _imageRepo.UpdateAsync(entity);
                    return new ResponseResult<bool>(result, result ? null : "Failed to update image.", result);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<bool>(false, ex.Message, false);
                }
            }

            // 5️⃣ Delete (toggle)
            public async Task<ResponseResult<bool>> DeleteAsync(DeleteProductImageUrlVM model)
            {
                try
                {
                    var existing = await _imageRepo.GetByIdAsync(model.Id);
                    if (existing == null)
                        return new ResponseResult<bool>(false, "Image not found", false);

                    bool toggle = await _imageRepo.ToggleDeleteStatusAsync(model.Id, model.DeletedBy ?? "system");
                    if (!toggle)
                        return new ResponseResult<bool>(false, "Failed to delete image.", false);

                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        await Upload.RemoveFileAsync("ProductImages", existing.ImageUrl);
                    }

                    return new ResponseResult<bool>(true, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<bool>(false, ex.Message, false);
                }
            }
        }
    }


