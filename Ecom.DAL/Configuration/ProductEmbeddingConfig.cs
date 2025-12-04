

namespace Ecom.DAL.Configuration
{
    public class ProductEmbeddingConfig : IEntityTypeConfiguration<ProductEmbedding>
    {
        public void Configure(EntityTypeBuilder<ProductEmbedding> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Vector)
                   .IsRequired()
                   .HasColumnType("varbinary(max)");

            builder.Property(e => e.ModelName)
                   .HasMaxLength(100);

            builder.HasOne(e => e.Product)
                   .WithMany(p => p.ProductEmbeddings)
                   .HasForeignKey(e => e.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
