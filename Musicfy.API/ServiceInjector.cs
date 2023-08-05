namespace Musicfy.API
{
    public static class ServiceInjector
    {
        public static void InjectServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Context
            services.AddDbContext<MusicfyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();

            // Serilog
            builder.Host.UseSerilog();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Cache
            builder.Services.AddOutputCache(options =>
            {
                //options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(10)));
                options.AddPolicy("Categories", policy => policy.Tag("Categories").Expire(TimeSpan.FromHours(1)));
                options.AddPolicy("Artists", policy => policy.Tag("Artists").Expire(TimeSpan.FromHours(1)));
                options.AddPolicy("Category", policy => policy.SetVaryByQuery("key").Tag("Categories").Expire(TimeSpan.FromHours(1)));
                options.AddPolicy("Artist", policy => policy.SetVaryByQuery("key").Tag("Artists").Expire(TimeSpan.FromHours(1)));

            });

            // Cors
            builder.Services.AddCors();
        }
    }
}
