
namespace Ecom.DAL.Configuration
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.Property(ci => ci.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(ci => ci.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            builder.HasOne(ci => ci.Product)
                   .WithMany()
                   .HasForeignKey(ci => ci.ProductId);

        }
    }
}
