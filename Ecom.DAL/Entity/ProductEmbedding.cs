
using System.ComponentModel.DataAnnotations;

namespace Ecom.DAL.Entity
{
    public class ProductEmbedding
    {
        public int Id { get; private set; }
        public string? SourceText { get; private set; }

        // The raw embedding vector stored as bytes
        public byte[] Vector { get; private set; } = Array.Empty<byte>();

        public string ModelName { get; private set; } = "nomic-embed-text-v1.5";

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public string? CreatedBy { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }

        public int ProductId { get; private set; }
        // Navigation
        public virtual Product? Product { get; private set; }

        
        public ProductEmbedding() { }

        public ProductEmbedding(int productId, string sourceText, byte[] vector, string createdBy, string modelName)
        {
            ProductId = productId;
            SourceText = sourceText;
            Vector = vector;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            ModelName = modelName;
        }

        public bool Update(byte[] newVector, string? newSourceText, string updatedBy)
        {
            if (newVector == null || newVector.Length == 0 || string.IsNullOrEmpty(updatedBy))
                return false;

            Vector = newVector;
            if (!string.IsNullOrWhiteSpace(newSourceText))
                SourceText = newSourceText;

            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            return true;
        }
    }
}
