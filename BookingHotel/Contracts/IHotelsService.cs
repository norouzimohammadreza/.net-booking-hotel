using BookingHotel.DTOs.Hotel;
using BookingHotel.DTOs.Pagination;
using BookingHotel.Results;

namespace BookingHotel.Contracts;

public interface IHotelsService
{
    Task<Result<PagedResult<GetHotelsDto>>>  GetHotels(PaginationQuery pagination);
    Task<Result<GetHotelDto>> GetHotel(int id);
    Task<Result<GetHotelDto>> CreateHotel(CreateHotelDto hotelDto);
    Task<Result> UpdateHotel(int id, UpdateHotelDto hotelDto);
    Task<Result> DeleteHotel(int id);
}