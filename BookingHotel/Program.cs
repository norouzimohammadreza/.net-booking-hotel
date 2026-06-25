using BookingHotel.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 var connectionString = builder.Configuration.GetConnectionString("BookingHotelDbConnectionString");
builder.Services.AddDbContext<BookingHotelDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();  
// Learn more about conf iguring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();