using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.Service.Implementation
{
    public class RatingCalculatorService : IRatingCalculatorService
    {
        private readonly IProductReviewRepo _reviewRepo;

        public RatingCalculatorService(IProductReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<decimal> CalculateAverageRatingAsync(int productId)
        {
            var reviews = await _reviewRepo.GetAllAsync(
                r => !r.IsDeleted && r.ProductId == productId
            );

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }

}
