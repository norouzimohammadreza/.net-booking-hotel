using BookingHotel.DTOs.Hotel;

namespace BookingHotel.DTOs.Country;

public record GetCountryDto(
    int Id,
    string Name,
    string ShortName,
    List<GetHotelsDto>? Hotels);