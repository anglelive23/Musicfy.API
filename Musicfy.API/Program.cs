var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddOData(options =>
{
    options.AddRouteComponents("api/odata", new MusicfyEntityDataModel().GetEntityDataModel()).Select().Filter().OrderBy().Expand().SetMaxTop(1000);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.InjectServices(builder);

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
