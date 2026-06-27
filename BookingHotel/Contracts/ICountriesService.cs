using BookingHotel.DTOs.Country;

namespace BookingHotel.Contracts;

public interface ICountriesService
{
    Task<IEnumerable<GetCountriesDto>> GetCountries(); 
    Task<GetCountryDto?> GetCountry(int countryId);
    Task<GetCountryDto> UpdateCountryDto(int countryId, UpdateCountryDto countryDto);
    Task<GetCountryDto> CreateCountryDto(CreateCountryDto countryDto);
    Task DeleteCountryDto(int countryId);
} 