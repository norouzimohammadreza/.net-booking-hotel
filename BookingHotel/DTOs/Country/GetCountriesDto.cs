namespace BookingHotel.DTOs.Country;

public record GetCountriesDto(
    int CountryId,
    string Name,
    string ShortName
);