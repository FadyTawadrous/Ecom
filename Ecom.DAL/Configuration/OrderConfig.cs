
namespace Ecom.DAL.Configuration
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .HasConversion<string>(); // Stores enum as string

            builder.Property(o => o.TotalAmount)
               .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.AppUser)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.AppUserId);

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // If you delete an Order, delete its Items

        }
    }
}
