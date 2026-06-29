using System.Text.Json.Serialization;
using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 var connectionString = builder.Configuration.GetConnectionString("BookingHotelDbConnectionString");
builder.Services.AddDbContext<BookingHotelDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });  

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();
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