
using Ecom.BLL.Service.Implementation;
using Ecom.DAL.Entity;
using Ecom.DAL.Seeding;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Stripe;

namespace Ecom.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // Add services to the container.

            // Database Context Configuration
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();

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

            // This tells .NET: "If a request comes in starting with /Files, 
            // look inside the physical 'Files' folder in the project root."
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "Files")),
                RequestPath = "/Files"
            });


            app.UseHangfireDashboard("/BackgroundJobs");
            RecurringJob.AddOrUpdate<ICartReminderService>(
                "abandoned-cart-email",
                job => job.SendAbandonedCartEmailsAsync(),
                Cron.Daily(12, 0));

            app.MapControllers();

            //comment
            app.Run();
        }
    }
}