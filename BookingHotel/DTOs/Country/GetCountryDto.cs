namespace BookingHotel.DTOs.Country;

public record GetCountryDto(
    int CountryId,
    string Name,
    string ShortName
);