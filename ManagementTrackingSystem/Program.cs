using ManagementTrackingSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    // Optionally, configure JSON options or other formatter settings
    .AddJsonOptions(options =>
    {
        // Configure JSON serializer settings to keep the original names in serialization and deserialization
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Register the AutoMapper with dependency injection
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register the ProductDbContext with dependency injection
builder.Services.AddDbContext<ManagementTrackingSystemDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ManagementTrackingSystemDbConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
