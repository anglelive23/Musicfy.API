var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddOData(options =>
{
    options.AddRouteComponents("api/odata", new MusicfyEntityDataModel().GetEntityDataModel()).Select().Filter().OrderBy().Expand().SetMaxTop(1000);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dbContext
builder.Services.AddDbContext<MusicfyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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
    options.AddPolicy("Category", policy => policy.SetVaryByQuery("key").Tag("Categories").Expire(TimeSpan.FromHours(1)));

});

// Cors
builder.Services.AddCors();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapControllers();
app.UseOutputCache();
app.Run();
