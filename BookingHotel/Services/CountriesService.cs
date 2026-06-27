using BookingHotel.Data;
using BookingHotel.DTOs.Country;
using BookingHotel.Contracts;
using BookingHotel.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Services;

public class CountriesService(BookingHotelDbContext context) : ICountriesService
{
    public async Task<IEnumerable<GetCountriesDto>> GetCountries()
    {
        return await context
            .Countries
            .Select(c=> new GetCountriesDto(c.CountryId, c.Name,c.ShortName))
            .ToListAsync(); 
    }

    public async Task<GetCountryDto?> GetCountry(int id)
    {
        var country = await context
            .Countries
            .Where(c=> c.CountryId == id)
            .Select(c=> new GetCountryDto(
                c.CountryId,
                c.Name,
                c.ShortName,
                c.Hotels.Select(h=> new GetHotelsDto(
                        h.Id,
                        h.Name,
                        h.Address,
                        h.Rating,
                        h.CountryId
                    )
                ).ToList()
            ))
            .FirstOrDefaultAsync();

        return country ?? null;
    }

    public async Task<GetCountryDto> UpdateCountryDto(int id, UpdateCountryDto countryDto)
    {
         
        var country = await context.Countries.FindAsync(countryDto.CountryId);
        if (country == null)
        {
            throw new KeyNotFoundException();
        }

        country.Name = countryDto.Name;
        country.ShortName = countryDto.ShortName;

        context.Entry(country).State = EntityState.Modified;
        
        await context.SaveChangesAsync();

        return new GetCountryDto(
            country.CountryId,
            country.Name,
            country.ShortName,
            []
            );
    }

    public async Task<GetCountryDto> CreateCountryDto(CreateCountryDto countryDto)
    {
        var country = new Country
        {
            Name = countryDto.Name,
            ShortName = countryDto.ShortName,
        };
        context.Countries.Add(country);
        await context.SaveChangesAsync();
        return new GetCountryDto(
            country.CountryId,
            country.Name,
            country.ShortName,
            []
        );
    }

    public async Task DeleteCountryDto(int countryId)
    {
        var country = await context.Countries.FindAsync(countryId) ?? throw  new KeyNotFoundException();
        context.Countries.Remove(country);
        await context.SaveChangesAsync();
    }
}
