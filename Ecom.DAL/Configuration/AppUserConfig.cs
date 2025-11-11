
namespace Ecom.DAL.Configuration
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(u => u.Orders)
               .WithOne(o => o.AppUser)
               .HasForeignKey(o => o.AppUserId);

            builder.HasOne(u => u.Cart)
               .WithOne(c => c.AppUser)
               .HasForeignKey<Cart>(c => c.AppUserId);

            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.AppUser)
                .HasForeignKey(a => a.AppUserId);

            builder.HasMany(u => u.ProductReviews)
                .WithOne(r => r.AppUser)
                .HasForeignKey(r => r.AppUserId);

            builder.HasMany(u => u.WishlistItems)
                .WithOne(w => w.AppUser)
                .HasForeignKey(w => w.AppUserId);
        }
    }
}
