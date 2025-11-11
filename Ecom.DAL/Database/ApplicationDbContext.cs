
namespace Ecom.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

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

        
    }
}
