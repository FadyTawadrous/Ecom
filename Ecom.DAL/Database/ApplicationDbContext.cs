
using System.Reflection;

namespace Ecom.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Register entity config
            //modelBuilder.ApplyConfiguration(new EmployeeConfig());

            // Alternatively, you can use the following line to automatically apply all configurations from the assembly
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImageUrl> ProductImageUrls { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
    }
}
