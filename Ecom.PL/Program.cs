
using Ecom.DAL.Entity;
using Ecom.DAL.Seeding;
using Microsoft.AspNetCore.Identity;

namespace Ecom.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Database Context Configuration
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // In-Memory database for testing and development
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseInMemoryDatabase("MyDB"));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Modular DAL and BLL Services
            builder.Services.AddBusinessInDAL();
            builder.Services.AddBusinessInBLL(builder.Configuration);

            // Add CORS Policy
            var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(",");
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins(allowedOrigins);
                });
            });

            var app = builder.Build();

            //Run your seeder here
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var context = services.GetRequiredService<ApplicationDbContext>();
            //        var userManager = services.GetRequiredService<UserManager<AppUser>>();
            //        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            //        Seeder.SeedAsync(context, userManager, roleManager).Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred during migration or seeding.");
            //    }
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use CORS Middleware
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}