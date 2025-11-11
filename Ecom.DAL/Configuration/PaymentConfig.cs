
namespace Ecom.DAL.Configuration
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentMethod)
                .HasConversion<string>();

            builder.Property(p => p.Status)
                .HasConversion<string>();

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
