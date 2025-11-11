
namespace Ecom.DAL.Configuration
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId);

            builder.Property(oi => oi.UnitPrice)
               .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
