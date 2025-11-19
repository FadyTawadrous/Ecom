
namespace Ecom.DAL.Database
{
    public static class DbSeeder
    {
        // Note: This is NOT recommended practice.
        public static void Seed(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            try
            {
                // --- 1. Create Migrations (Sync) ---
                // This blocks the thread until the database is created
                //context.Database.Migrate();

                // --- 2. Seed Admin User (Sync) ---
                //if (!userManager.Users.Any(u => u.Email == "admin@ecom.com"))
                //{
                //    var adminUser = new AppUser(
                //        email: "admin@ecom.com",
                //        displayName: "Admin User",
                //        profileImageUrl: null,
                //        createdBy: "System",
                //        phoneNumber: "123456789"
                //    );

                //    // Use the synchronous 'Create' and check the result
                //    var result = userManager.CreateAsync(adminUser, "P@ssword123");
                //    if (!result.Succeeded)
                //    {
                //        // Handle the error (e.g., password not strong enough)
                //        throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                //    }
                //    // Note: You would also add to "Admin" role here
                //}

                // --- 3. Seed a Test Brand (Sync) ---
                if (!context.Brands.Any())
                {
                    var brand = new Brand(
                        name: "Test Brand",
                        imageUrl: null,
                        createdBy: "System"
                    );
                    context.Brands.Add(brand);
                }

                // --- 4. Seed a Test Category (Sync) ---
                if (!context.Categories.Any())
                {
                    var category = new Category(
                        name: "Test Category",
                        imageUrl: null,
                        createdBy: "System"
                    );
                    context.Categories.Add(category);
                }

                // --- 5. Save all changes (Sync) ---
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Since this is sync, we can just let the exception bubble up
                // to Program.cs
                throw new Exception("An error occurred during synchronous seeding.", ex);
            }
        }
    }
}