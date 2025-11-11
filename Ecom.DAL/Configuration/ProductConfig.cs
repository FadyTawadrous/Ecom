
namespace Ecom.DAL.Configuration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            builder.Property(p => p.Rating)
                .HasColumnType("decimal(3,2)");

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.HasMany(p => p.ProductImageUrls)
                   .WithOne(piu => piu.Product)
                   .HasForeignKey(piu => piu.ProductId);

            builder.HasMany(p => p.ProductReviews)
                   .WithOne(pr => pr.Product)
                   .HasForeignKey(pr => pr.ProductId);

            builder.HasMany(p => p.WishlistItems)
                   .WithOne(wi => wi.Product)
                   .HasForeignKey(wi => wi.ProductId);
        }

    }
}
