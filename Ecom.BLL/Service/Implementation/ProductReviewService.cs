using Ecom.BLL.ModelVM.ProductReview;
using Ecom.BLL.Responses;
using Ecom.BLL.Service.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecom.BLL.Service.Implementation
{
    // BLL/Service/Implementation/ProductReviewService.cs
   

   
    
        public class ProductReviewService : IProductReviewService
        {
            private readonly IProductReviewRepo _reviewRepo;
            private readonly IProductRepo _productRepo;
            private readonly IMapper _mapper;
            private readonly IRatingCalculatorService _ratingCalculatorService;
            private readonly IProductService _productService;

            public ProductReviewService(
            IProductReviewRepo reviewRepo,
            IProductRepo productRepo,
            IMapper mapper,
            IRatingCalculatorService ratingCalculatorService,
            IProductService productService
            )
        {
            _reviewRepo = reviewRepo;
            _productRepo = productRepo;
            _mapper = mapper;
            _ratingCalculatorService = ratingCalculatorService;
            _productService = productService;
        }

        // ----------------------------
        // 1) Get all reviews
        // ----------------------------
        public async Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetAllAsync()
            {
                try
                {
                    var reviews = await _reviewRepo.GetAllAsync(
                        r => !r.IsDeleted,
                        r => r.Product,
                        r => r.AppUser
                    );

                    var mapped = _mapper.Map<IEnumerable<ProductReviewGetVM>>(reviews);
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(null, ex.Message, false);
                }
            }

            // ----------------------------
            // 2) Get all reviews for a specific product
            // ----------------------------
            public async Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetByProductIdAsync(int productId)
            {
                try
                {
                    var reviews = await _reviewRepo.GetAllAsync(
                        r => !r.IsDeleted && r.ProductId == productId,
                        r => r.Product,
                        r => r.AppUser
                    );

                    var mapped = _mapper.Map<IEnumerable<ProductReviewGetVM>>(reviews);
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(null, ex.Message, false);
                }
            }

            // ----------------------------
            // 3) Get all reviews for a specific user
            // ----------------------------
            public async Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetByUserIdAsync(string userId)
            {
                try
                {
                    var reviews = await _reviewRepo.GetAllAsync(
                        r => !r.IsDeleted && r.AppUserId == userId,
                        r => r.Product,
                        r => r.AppUser
                    );

                    var mapped = _mapper.Map<IEnumerable<ProductReviewGetVM>>(reviews);
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(null, ex.Message, false);
                }
            }

            // ----------------------------
            // 4) Get all reviews for a brand (via product navigation)
            // ----------------------------
            public async Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetByBrandIdAsync(int brandId)
            {
                try
                {
                    // filter by navigation property Product.BrandId inside predicate
                    var reviews = await _reviewRepo.GetAllAsync(
                        r => !r.IsDeleted && r.Product.BrandId == brandId,
                        r => r.Product,
                        r => r.AppUser
                    );

                    var mapped = _mapper.Map<IEnumerable<ProductReviewGetVM>>(reviews);
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<IEnumerable<ProductReviewGetVM>>(null, ex.Message, false);
                }
            }

            // ----------------------------
            // 5) Get review by Id
            // ----------------------------
            public async Task<ResponseResult<ProductReviewGetVM>> GetByIdAsync(int id)
            {
                try
                {
                    var review = await _reviewRepo.GetByIdAsync(id);
                    if (review == null || review.IsDeleted)
                        return new ResponseResult<ProductReviewGetVM>(null, "Review not found", false);

                    var mapped = _mapper.Map<ProductReviewGetVM>(review);
                    return new ResponseResult<ProductReviewGetVM>(mapped, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<ProductReviewGetVM>(null, ex.Message, false);
                }
            }

            // ----------------------------
            // 6) Create review and update product rating
            // ----------------------------
            public async Task<ResponseResult<bool>> CreateAsync(ProductReviewCreateVM model)
            {
                try
                {
                    // Map VM -> Entity using ConstructUsing mapping
                    var entity = _mapper.Map<ProductReview>(model);

                    var added = await _reviewRepo.AddAsync(entity);
                    if (!added)
                        return new ResponseResult<bool>(false, "Failed to add review", false);

                // Recalculate & update product rating (product repo will compute average)
                // STEP 1: Calculate new rating
                decimal avgRating = await _ratingCalculatorService.CalculateAverageRatingAsync(entity.ProductId);

                // STEP 2: Update product rating
                await _productService.UpdateRatingAsync(entity.ProductId, avgRating);

                return new ResponseResult<bool>(true, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<bool>(false, ex.Message, false);
                }
            }

            // ----------------------------
            // 7) Update review and update product rating
            // ----------------------------
            public async Task<ResponseResult<bool>> UpdateAsync(ProductReviewUpdateVM model)
            {
                try
                {
                    var existing = await _reviewRepo.GetByIdAsync(model.Id);
                    if (existing == null)
                        return new ResponseResult<bool>(false, "Review not found", false);

                    // Map update properties into tracked entity (mapper.Map(src, dest))
                    _mapper.Map(model, existing);

                    var updated = await _reviewRepo.UpdateAsync(existing);
                    if (!updated)
                        return new ResponseResult<bool>(false, "Failed to update review", false);


                decimal avgRating = await _ratingCalculatorService.CalculateAverageRatingAsync(existing.ProductId);

                // 🔹 STEP 2: Update product rating
                await _productService.UpdateRatingAsync(existing.ProductId, avgRating);

                return new ResponseResult<bool>(true, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<bool>(false, ex.Message, false);
                }
            }

            // ----------------------------
            // 8) Toggle delete/restore review and update product rating
            // ----------------------------
            public async Task<ResponseResult<bool>> ToggleDeleteAsync(int id, string userModified)
            {
                try
                {
                    var existing = await _reviewRepo.GetByIdAsync(id);
                    if (existing == null)
                        return new ResponseResult<bool>(false, "Review not found", false);

                    var toggled = await _reviewRepo.ToggleDeleteStatusAsync(id, userModified ?? "system");
                    if (!toggled)
                        return new ResponseResult<bool>(false, "Failed to toggle delete status", false);

                decimal avgRating = await _ratingCalculatorService.CalculateAverageRatingAsync(existing.ProductId);

                // 🔹 STEP 2: Update product rating
                await _productService.UpdateRatingAsync(existing.ProductId, avgRating);

                return new ResponseResult<bool>(true, null, true);
                }
                catch (Exception ex)
                {
                    return new ResponseResult<bool>(false, ex.Message, false);
                }
            }

        
    }
    }


