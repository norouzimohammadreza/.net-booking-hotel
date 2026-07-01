using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Results;

namespace BookingHotel.Services;

public class HotelsService(BookingHotelDbContext context) : IHotelsService
{
    public async Task<Result<IEnumerable<GetHotelsDto>>> GetHotels()
    {
        var hotels = await context
            .Hotels
            .Select(h=> new GetHotelsDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId))
            .ToListAsync();
        
        return Result<IEnumerable<GetHotelsDto>>.Success(hotels);
    }

    public async Task<Result<GetHotelDto>> GetHotel(int id)
    {
      var hotel = await context
            .Hotels
            .Where(h => h.Id == id)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.Country!.Name))
            .FirstOrDefaultAsync();
      
      return hotel == null ? Result<GetHotelDto>.NotFound() : Result<GetHotelDto>.Success(hotel);
      
    }

    public async Task<Result<GetHotelDto>> CreateHotel(CreateHotelDto hotelDto)
    {
        try
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

            var dto = new GetHotelDto(
                hotel.Id,
                hotel.Name,
                hotel.Address,
                hotel.Rating,
                hotel.Country!.Name
            );
            return Result<GetHotelDto>.Success(dto);
        }
        catch (Exception)
        {
            return Result<GetHotelDto>.Failure();
        }

    }

    public async Task<Result> UpdateHotel(int id, UpdateHotelDto hotelDto)
    {
        try
        {
            if (id != hotelDto.Id)
            {
                return Result.Failure(new Error("Validation","Validation Error"));
            }
            var hotel = await context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return Result.Failure(new Error("NotFound", "NotFound"));
            }
            hotel.Name = hotelDto.Name;
            hotel.Address = hotelDto.Address;
            hotel.Rating = hotelDto.Rating;
            hotel.CountryId = hotelDto.CountryId;

            context.Entry(hotel).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure();
        }

    }

    public async Task<Result> DeleteHotel(int id)
    {
        try
        {
            var hotel = await context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return Result.Failure(new Error("NotFound", "NotFound"));
            }
            context.Hotels.Remove(hotel);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure();
        }

    }
}