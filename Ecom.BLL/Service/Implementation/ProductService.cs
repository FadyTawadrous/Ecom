using Ecom.BLL.ModelVM.Product;
using Ecom.BLL.ModelVM.ProductReview;
using Ecom.DAL.Database;
using Ecom.DAL.Entity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly IProductReviewService _productReviewService;

        public ProductService(IProductRepo productRepo, IMapper mapper, IProductReviewService productReviewService)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _productReviewService = productReviewService;
        }


        //1️⃣ GetAllForAdminAsync()

       // Returns all products, but only those NOT soft-deleted.
        public async Task<ResponseResult<IEnumerable<GetProductVM>>> GetAllForAdminAsync()
        {
            try
            {
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted,
                    p => p.ProductImageUrls,
                    p => p.ProductReviews,
                    p => p.Brand,
                    p => p.Category
                );

                var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);

                return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetProductVM>>(null, ex.Message, false);
            }
        }

        //        2️⃣ GetAllByBrandIdAsync(int brandId)

        //Returns all products belonging to a specific brand.

        public async Task<ResponseResult<IEnumerable<GetProductVM>>> GetAllByBrandIdAsync(int brandId)
        {
            try
            {
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted && p.BrandId == brandId,
                    p => p.ProductImageUrls,
                    p => p.ProductReviews,
                    p => p.Brand,
                    p => p.Category
                );

                var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);

                return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetProductVM>>(null, ex.Message, false);
            }
        }


            //        3️⃣ GetAllByCategoryIdAsync(int categoryId)

            //Returns all products inside a specific category.

        public async Task<ResponseResult<IEnumerable<GetProductVM>>> GetAllByCategoryIdAsync(int categoryId)
        {
            try
            {
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted && p.CategoryId == categoryId,
                    p => p.ProductImageUrls,
                    p => p.ProductReviews,
                    p => p.Brand,
                    p => p.Category
                );

                var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);

                return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetProductVM>>(null, ex.Message, false);
            }
        }




        public async Task<ResponseResult<GetProductVM>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product == null || product.IsDeleted)
                    return new ResponseResult<GetProductVM>(null, "Product not found", false);

                var mapped = _mapper.Map<GetProductVM>(product);
                var reviewResponse = await _productReviewService.GetByProductIdAsync(id);

                if (reviewResponse.IsSuccess)
                {
                    mapped.Reviews = reviewResponse.Result;
                }
                else
                {
                    mapped.Reviews = new List<ProductReviewGetVM>(); // or null, your choice
                }
                return new ResponseResult<GetProductVM>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<GetProductVM>(null, ex.Message, false);
            }
        }

      

        public async Task<ResponseResult<bool>> CreateAsync(CreateProductVM model)
        {
            try
            {
                // 1. Validate product (e.g., product title unique) - optional
                // 2. Handle thumbnail upload
                string thumbnailUrl =  "default.png";
                if (model.Thumbnail != null)
                {
                    try
                    {
                        thumbnailUrl = await Upload.UploadFileAsync("Images/ProductThumbnail", model.Thumbnail);
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<bool>(false, $"Thumbnail upload failed: {ex.Message}", false);
                    }
                }

                model.ThumbnailUrl = thumbnailUrl;

                // 3. Map VM -> Entity (use ConstructUsing in AutoMapper profile)
                var entity = _mapper.Map<Product>(model);

                // 4. Persist

                var result = await _productRepo.AddAsync(entity);
                if (!result)
                {
                    // cleanup uploaded file on failure
                    if (!string.IsNullOrEmpty(thumbnailUrl) && thumbnailUrl != "default.png")
                        await Upload.RemoveFileAsync("Images/ProductThumbnail", thumbnailUrl);

                    return new ResponseResult<bool>(false, "Failed to save product.", false);
                }

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

       

        public async Task<ResponseResult<bool>> UpdateAsync(UpdateProductVM model)
        {
            try
            {
                var existing = await _productRepo.GetByIdAsync(model.Id);
                if (existing == null) return new ResponseResult<bool>(false, "Product not found", false);

                // thumbnail replacement
                string newThumb = existing.ThumbnailUrl;
                if (model.Thumbnail != null)
                {
                    try
                    {
                        newThumb = await Upload.UploadFileAsync("Images/ProductThumbnail", model.Thumbnail);
                        if (!string.IsNullOrEmpty(existing.ThumbnailUrl) && existing.ThumbnailUrl != "default.png")
                        {
                            await Upload.RemoveFileAsync("Images/ProductThumbnail", existing.ThumbnailUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ResponseResult<bool>(false, $"Thumbnail update failed: {ex.Message}", false);
                    }
                }

                model.ThumbnailUrl = newThumb;
                _mapper.Map(model, existing);

                var result = await _productRepo.UpdateAsync(existing);
                return new ResponseResult<bool>(result, result ? null : "Failed to update product.", result);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

       

        public async Task<ResponseResult<bool>> DeleteAsync(DeleteProductVM model)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(model.Id);
                if (product == null)
                    return new ResponseResult<bool>(false, "Product not found", false);

                bool toggled = await _productRepo.ToggleDeleteStatusAsync(model.Id, model.DeletedBy ?? "system");
                if (!toggled)
                    return new ResponseResult<bool>(false, "Failed to toggle delete status", false);

                // optionally remove thumbnail
                if (!string.IsNullOrEmpty(product.ThumbnailUrl) && product.ThumbnailUrl != "default.png")
                {
                    await Upload.RemoveFileAsync("Images/ProductThumbnail", product.ThumbnailUrl);
                }

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }


        //Decrease stock
        public async Task<ResponseResult<bool>> DecreaseStockAsync(int productId, int quantity)
        {
            try
            {
                var success = await _productRepo.DecreaseStockAsync(productId, quantity);
                if (!success)
                    return new ResponseResult<bool>(false, "Not enough stock", false);

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        //increase stock
        public async Task<ResponseResult<bool>> IncreaseStockAsync(int productId, int quantity)
        {
            try
            {
                var success = await _productRepo.IncreaseStockAsync(productId, quantity);
                if (!success)
                    return new ResponseResult<bool>(false, "Failed to restore stock", false);

                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<bool> UpdateRatingAsync(int productId, decimal newAverageRating)
        {
            return await _productRepo.UpdateRatingAsync(productId, newAverageRating);
        }

        public async Task<ResponseResult<bool>> AddToQuantitySoldAsync(AddQuantitySoldVM model)
        {
            try
            {
                // STEP 1: Get product
                var product = await _productRepo.GetByIdAsync(model.ProductId);
                if (product == null)
                    return new ResponseResult<bool>(false, "Product not found", false);

                // STEP 2: Apply QuantitySold change
                bool updated = await _productRepo.AddToQuantitySoldAsync(model.ProductId, model.QuantitySold);
                if (!updated)
                    return new ResponseResult<bool>(false, "Failed to update QuantitySold", false);

                // STEP 3: Success
                return new ResponseResult<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<bool>(false, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetProductVM>>> SearchByTitleAsync(string title)
        {
            try
            {
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted && p.Title.Contains(title),
                    p => p.ProductImageUrls,
                    p => p.ProductReviews,
                    p => p.Brand,
                    p => p.Category
                );

                var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);
                return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetProductVM>>(null, ex.Message, false);
            }
        }


        public async Task<ResponseResult<IEnumerable<GetProductVM>>> SearchByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted && p.Price >= minPrice && p.Price <= maxPrice,
                    p => p.ProductImageUrls,
                    p => p.ProductReviews,
                    p => p.Brand,
                    p => p.Category
                );

                var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);
                return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetProductVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetProductVM>>> SearchByRatingAsync(decimal minRating)
        {
            try
            {
                var products = await _productRepo.GetAllAsync(
                    p => !p.IsDeleted && p.Rating >= minRating,
                    p => p.ProductImageUrls,
                    p => p.ProductReviews,
                    p => p.Brand,
                    p => p.Category
                );

                var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);
                return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<GetProductVM>>(null, ex.Message, false);
            }
        }

        public async Task<ResponseResult<IEnumerable<GetProductVM>>> GetFilteredProductsAsync(ProductFilterDto filter)
        {
            // Build dynamic filter expression
            Expression<Func<Product, bool>>? filterExpression = p =>
              (filter.BrandId == null || p.BrandId == filter.BrandId) &&
                (filter.CategoryId == null || p.CategoryId == filter.CategoryId) &&
                (filter.MinPrice == null || p.Price >= filter.MinPrice) &&
                (filter.MaxPrice == null || p.Price <= filter.MaxPrice) &&
               (filter.MinRating == null || p.Rating >= (decimal)filter.MinRating) &&
                (string.IsNullOrEmpty(filter.Search) || p.Title.Contains(filter.Search));

            // Use repo flexible method
            var products = await _productRepo.GetAllAsync(filterExpression,
                p => p.Brand,
                p => p.Category,
                p => p.ProductImageUrls
            );

            

            var mapped = _mapper.Map<IEnumerable<GetProductVM>>(products);

            return new ResponseResult<IEnumerable<GetProductVM>>(mapped, null, true);
        }

       



    }

}
