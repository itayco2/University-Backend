using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace University_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Building app:
            var builder = WebApplication.CreateBuilder(args);
            AppConfig.Configure(builder.Environment);

            // Configure database connection
            var connectionString = builder.Configuration.GetConnectionString("UniversityDB");
            builder.Services.AddDbContext<UniversityContext>(options => options.UseSqlServer(connectionString));


            // Blocks: 
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("RateLimit")); // Configuration from appsettings.json
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>(); // Create rate limit object.
            builder.Services.AddInMemoryRateLimiting(); // Save rate limit calculations in memory and not in some database.


            // Create object for DIs:
            builder.Services.AddDbContext<UniversityContext>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<LessonService>();
            builder.Services.AddScoped<CourseService>();



            builder.Services.AddMvc(options => options.Filters.Add<CatchAllFilter>()); // Adding catch-all filter to entire app.
            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true); // Don't check validation errors using built-in filter, we're going to check them.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtHelper.SetBearerOptions); // Use JWT as the authentication mechanism.
            builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);




            var app = builder.Build();


            // Configure and run app:
            app.UseIpRateLimiting(); // RateLimit middleware.
            app.UseAuthorization(); // Middleware for Auth
            app.MapControllers();
            app.Run();
        }
    }
}
