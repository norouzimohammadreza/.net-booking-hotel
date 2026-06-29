using BookingHotel.DTOs.Hotel;

namespace BookingHotel.Contracts;

public interface IHotelsService
{
    Task<IEnumerable<GetHotelsDto>> GetHotels();
    Task<GetHotelDto?> GetHotel(int id);
    Task<GetHotelDto> CreateHotel(CreateHotelDto hotelDto);
    Task UpdateHotel(int id, UpdateHotelDto hotelDto);
    Task DeleteHotel(int id);
}