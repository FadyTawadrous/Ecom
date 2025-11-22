
using System.Net.Http.Json;
using static Ecom.DAL.Seeding.DataSeedingModels;

namespace Ecom.DAL.Seeding
{
    public static class Seeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Before using the seeder:
            //1- Ensure the database connection string is correctly set in your configuration in appsetting.json.
            //2- Add migrations using the Package Manager Console or CLI: "Add-Migration InitialCreate"
            //3- Then "Update-Database" to create the database.
            //4- Make sure migration folder exists in the DAL project, and the database is created using SSMS.
            //5- Uncomment the seeding block in Program.cs to run the seeder.
            //6- After the initial seeding, you can comment out or remove the seeding block to prevent duplicate data insertion on subsequent runs.

            // 2. Seed Roles & Admin (Your previous code)
            await SeedUsersAndRolesAsync(context, userManager, roleManager);

            // 3. Seed Products from API
            await SeedProductsFromApiAsync(context, userManager);
        }

        private static async Task SeedUsersAndRolesAsync(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create Roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            // Create Admin User if it doesn't exist
            if (!await userManager.Users.AnyAsync(u => u.Email == "admin@ecom.com"))
            {
                var admin = new AppUser("admin@ecom.com", "System Admin", null, "System", null) { EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        private static async Task SeedProductsFromApiAsync(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            // 1. Check if we already have product data
            if (await context.Products.CountAsync() > 10) return;

            // 2. Fetch Data
            using var httpClient = new HttpClient();
            var response = await httpClient.GetFromJsonAsync<DummyResponse>("https://dummyjson.com/products?limit=100");

            if (response?.Products == null || response.Products.Count <= 0) return;

            var apiProducts = response.Products;

            // 3. Create a "Reviewer User" for the reviews
            // (Since the API reviews don't map to our real users, we assign them to a placeholder user)
            var reviewerUser = await userManager.FindByEmailAsync("reviewer@ecom.com");
            if (reviewerUser == null)
            {
                reviewerUser = new AppUser("reviewer@ecom.com", "Verified Buyer", null, "System", null) { EmailConfirmed = true };
                await userManager.CreateAsync(reviewerUser, "Password@123");
                await userManager.AddToRoleAsync(reviewerUser, "Customer");
            }

            // Since Brands and Categories are referenced by Products, we need to process them first.
            // 4. Process Brands (Extract distinct names, Save missing ones, Create Map)
            var distinctBrandNames = apiProducts
                .Select(p => p.Brand)
                .Where(b => !string.IsNullOrEmpty(b))
                .Distinct()
                .ToList();

            var existingBrands = await context.Brands.ToListAsync();
            var brandMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var brandName in distinctBrandNames)
            {
                var existing = existingBrands.FirstOrDefault(b => b.Name.Equals(brandName, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    // Create new Brand, save it to database to get the Id, and add the Id to the map
                    var newBrand = new Brand(brandName, null, "System");
                    context.Brands.Add(newBrand);
                    await context.SaveChangesAsync();
                    brandMap[brandName] = newBrand.Id;
                }
                else
                {
                    brandMap[brandName] = existing.Id;
                }
            }

            // 5. Process Categories (Same logic as Brands)
            var distinctCategories = apiProducts
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            var existingCategories = await context.Categories.ToListAsync();
            var categoryMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var catName in distinctCategories)
            {
                var existing = existingCategories.FirstOrDefault(c => c.Name!.Equals(catName, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    // Capitalize first letter for aesthetics
                    var formattedName = char.ToUpper(catName[0]) + catName.Substring(1);
                    var newCat = new Category(formattedName, null, "System");
                    context.Categories.Add(newCat);
                    await context.SaveChangesAsync();
                    categoryMap[catName] = newCat.Id;
                }
                else
                {
                    categoryMap[catName] = existing.Id;
                }
            }

            // 6. Map and Save Products
            var productsToAdd = new List<Product>();

            foreach (var item in apiProducts)
            {
                // Safety check: if brand/cat somehow missing
                if (!brandMap.ContainsKey(item.Brand) || !categoryMap.ContainsKey(item.Category)) continue;

                // Map API Product to Product Entity using Brand and Category Ids from the maps
                var product = new Product(
                   title: item.Title,
                   description: item.Description,
                   price: item.Price,
                   discountPercentage: item.DiscountPercentage,
                   stock: item.Stock,
                   thumbnailUrl: item.Thumbnail,
                   createdBy: "System",
                   brandId: brandMap[item.Brand],
                   categoryId: categoryMap[item.Category]
               );

                // Update the rating using your entity method
                product.UpdateRating(item.Rating);

                context.Products.Add(product);
                await context.SaveChangesAsync(); // Save to generate Product.Id

                // Now that we have a product with Id, we can add Images and Reviews linked to this product
                // 1- Add Images
                if (item.Images != null)
                {
                    foreach (var url in item.Images)
                    {
                        context.ProductImageUrls.Add(new ProductImageUrl(url, product.Id, "System"));
                    }
                }

                // 2- Add Reviews
                if (item.Reviews != null)
                {
                    foreach (var rev in item.Reviews)
                    {
                        // Map API Review to Entity
                        // Let's assume Title = "Review by Name" and Description = Comment
                        context.ProductReviews.Add(new ProductReview(
                            title: $"Review by {rev.ReviewerName}",
                            description: rev.Comment,
                            rating: rev.Rating,
                            createdBy: "System",
                            productId: product.Id,
                            appUserId: reviewerUser.Id
                        ));
                    }
                }
            }

            // Final Save for images and reviews
            await context.SaveChangesAsync();
        }
    }
}
