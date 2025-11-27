
using Microsoft.EntityFrameworkCore;

namespace Ecom.DAL.Configuration
{
    public class FaceIdConfig : IEntityTypeConfiguration<FaceId>
    {
        public void Configure(EntityTypeBuilder<FaceId> builder)
        {
            // Primary key
            builder.HasKey(f => f.Id);

            // Properties
            builder.Property(f => f.Encoding)
                   .IsRequired()
                   .HasColumnType("varbinary(max)"); // store byte[] as varbinary


            // Relationships
            builder.HasOne(f => f.AppUser)
                .WithMany(u => u.FaceIds)
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
