using BookingHotel.DTOs.Hotel;
using BookingHotel.Results;

namespace BookingHotel.Contracts;

public interface IHotelsService
{
    Task<Result<IEnumerable<GetHotelsDto>>>  GetHotels();
    Task<Result<GetHotelDto>> GetHotel(int id);
    Task<Result<GetHotelDto>> CreateHotel(CreateHotelDto hotelDto);
    Task<Result> UpdateHotel(int id, UpdateHotelDto hotelDto);
    Task<Result> DeleteHotel(int id);
}