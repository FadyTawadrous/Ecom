
namespace Ecom.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=EcomDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Register entity config
            //modelBuilder.ApplyConfiguration(new EmployeeConfig());

            // Alternatively, you can use the following line to automatically apply all configurations from the assembly
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        // Tables
        public DbSet<ProductImageUrl> ProductImageUrls { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
