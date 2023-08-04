using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Musicfy.API.Entities.Data;
using Musicfy.API.Entities.DataModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddOData(options => options.AddRouteComponents("api/odata", new MusicfyEntityDataModel().GetEntityDataModel()).Select().Filter().OrderBy().Count().Expand().SetMaxTop(100));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dbContext
builder.Services.AddDbContext<MusicfyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
