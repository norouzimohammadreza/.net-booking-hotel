using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Services;

public class HotelsService(BookingHotelDbContext context) : IHotelsService
{
    public async Task<IEnumerable<GetHotelsDto>> GetHotels()
    {
        return await context
            .Hotels
            .Select(h=> new GetHotelsDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId))
            .ToListAsync();
    }

    public async Task<GetHotelDto?> GetHotel(int id)
    {
      var country = await context
            .Hotels
            .Where(h => h.Id == id)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.Country!.Name))
            .FirstOrDefaultAsync();
      
      return country ?? null;
      
    }

    public async Task<GetHotelDto> CreateHotel(CreateHotelDto hotelDto)
    {
        var hotel = new Hotel
        {
            Name = hotelDto.Name,
            Address = hotelDto.Address,
            Rating = hotelDto.Rating,
            CountryId = hotelDto.CountryId
        };
        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        return new GetHotelDto(
            hotel.Id,
            hotel.Name,
            hotel.Address,
            hotel.Rating,
            hotel.Country!.Name
            );
    }

    public async Task UpdateHotel(int id, UpdateHotelDto hotelDto)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException();
        }
        hotel.Name = hotelDto.Name;
        hotel.Address = hotelDto.Address;
        hotel.Rating = hotelDto.Rating;
        hotel.CountryId = hotelDto.CountryId;

        context.Entry(hotel).State = EntityState.Modified;
       await context.SaveChangesAsync();
    }

    public async Task DeleteHotel(int id)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            throw new KeyNotFoundException();
        }
        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();
    }
}