

namespace Ecom.DAL.Repo.Implementation
{
    public class ProductEmbeddingRepo : IProductEmbeddingRepo
    {
        private readonly ApplicationDbContext _db;
        public ProductEmbeddingRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // Add a new ProductEmbedding
        public async Task<bool> AddAsync(ProductEmbedding embedding)
        {
            try
            {
                if (embedding == null)
                    return false;

                var result = await _db.ProductEmbeddings.AddAsync(embedding);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Delete all ProductEmbeddings
        public async Task<bool> DeleteAllAsync()
        {
            try
            {
                _db.ProductEmbeddings.RemoveRange(_db.ProductEmbeddings);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Delete a ProductEmbedding by ProductId
        public async Task<bool> DeleteByProductIdAsync(int productId)
        {
            try
            {
                if (productId == 0)
                    return false;

                var embedding = await _db.ProductEmbeddings.FirstOrDefaultAsync(e => e.ProductId == productId);
                if (embedding == null)
                    return false;
                _db.ProductEmbeddings.Remove(embedding);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get all ProductEmbeddings
        public async Task<IEnumerable<ProductEmbedding>> GetAllAsync()
        {
            try
            {
                // Include related Product, Brand, Category, and ProductReviews data
                // return all embeddings for products that are not deleted
                var embeddings = await _db.ProductEmbeddings
                                            .AsNoTracking()
                                            .Where(e => !e.Product!.IsDeleted)
                                            .Include(e => e.Product)
                                                .ThenInclude(p => p.Brand)
                                            .Include(e => e.Product)
                                                .ThenInclude(p => p.Category)
                                            .Include(e => e.Product)
                                                .ThenInclude(p => p.ProductReviews)
                                            .ToListAsync();
                return embeddings;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get a ProductEmbedding by ProductId
        public async Task<ProductEmbedding?> GetByProductIdAsync(int productId)
        {
            try
            {
                if (productId == 0)
                    return null;

                var embedding = await _db.ProductEmbeddings
                                            .Include(e => e.Product)
                                            .FirstOrDefaultAsync(e => e.ProductId == productId);
                if(embedding == null)
                    return null;

                return embedding;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Update an existing ProductEmbedding
        public async Task<bool> UpdateAsync(ProductEmbedding embedding)
        {
            try
            {
                if(embedding == null || embedding.Id == 0)
                    return false;

                var existingEmbedding = await _db.ProductEmbeddings.FirstOrDefaultAsync(e => e.Id == embedding.Id);
                if (existingEmbedding == null)
                    return false;

                bool isUpdated = existingEmbedding.Update(embedding.Vector
                                                        , embedding.SourceText
                                                        , embedding.UpdatedBy!);
                if (!isUpdated)
                    return false;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
