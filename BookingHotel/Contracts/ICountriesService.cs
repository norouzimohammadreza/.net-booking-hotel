using BookingHotel.DTOs.Country;
using BookingHotel.Results;

namespace BookingHotel.Contracts;

public interface ICountriesService
{
    Task<Result<IEnumerable<GetCountriesDto>>> GetCountries(); 
    Task<Result<GetCountryDto>> GetCountry(int countryId);
    Task<Result> UpdateCountry(int countryId, UpdateCountryDto countryDto);
    Task<Result<GetCountryDto>> CreateCountry(CreateCountryDto countryDto);
    Task<Result> DeleteCountryDto(int countryId);
} 