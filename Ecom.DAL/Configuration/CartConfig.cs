
namespace Ecom.DAL.Configuration
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.Ignore(c => c.TotalAmount);

            builder.HasOne(c => c.AppUser)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
