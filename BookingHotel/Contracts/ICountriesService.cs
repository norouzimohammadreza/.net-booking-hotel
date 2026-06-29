using BookingHotel.DTOs.Country;

namespace BookingHotel.Contracts;

public interface ICountriesService
{
    Task<IEnumerable<GetCountriesDto>> GetCountries(); 
    Task<GetCountryDto?> GetCountry(int countryId);
    Task UpdateCountry(int countryId, UpdateCountryDto countryDto);
    Task<GetCountryDto> CreateCountry(CreateCountryDto countryDto);
    Task DeleteCountryDto(int countryId);
} 