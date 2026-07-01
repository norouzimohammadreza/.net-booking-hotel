using BookingHotel.Data;
using BookingHotel.DTOs.Country;
using BookingHotel.Contracts;
using BookingHotel.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Results;

namespace BookingHotel.Services;

public class CountriesService(BookingHotelDbContext context) : ICountriesService
{
    public async Task<Result<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var countries = await context
            .Countries
            .Select(c=> new GetCountriesDto(c.CountryId, c.Name,c.ShortName))
            .ToListAsync(); 
        
        return Result<IEnumerable<GetCountriesDto>>.Success(countries);
    }

    public async Task<Result<GetCountryDto>> GetCountry(int id)
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

        return country==null ? Result<GetCountryDto>.NotFound() : Result<GetCountryDto>.Success(country);
    }

    public async Task<Result> UpdateCountry(int id, UpdateCountryDto countryDto)
    {
        try
        {
            if (id != countryDto.CountryId)
            {
                return Result.Failure(new Error("Validation","Country id does not match"));
            }
            var country = await context.Countries.FindAsync(countryDto.CountryId);
            if (country == null)
            {
                return Result.Failure(new Error("NotFound","Country is not find"));
            }
            
            var duplicateName = await context.Countries.AnyAsync(c=> c.CountryId != id && c.Name == countryDto.Name);

            if (duplicateName)
            {
                return Result.Failure(new Error("Duplicate","Country name already exists"));
            }

            country.Name = countryDto.Name;
            country.ShortName = countryDto.ShortName;

            context.Entry(country).State = EntityState.Modified;
        
            await context.SaveChangesAsync();
            return Result.Success();

        }
        catch (Exception)
        {
            return Result.Failure();
        }

        
    }

    public async Task<Result<GetCountryDto>> CreateCountry(CreateCountryDto countryDto)
    {
        try
        {
            var exists = await CountryExists(countryDto.Name);
            if (exists)
            {
                return Result<GetCountryDto>.Failure(new Error("Conflict","Country already exists"));
            }
            var country = new Country
            {
                Name = countryDto.Name,
                ShortName = countryDto.ShortName,
            };
            context.Countries.Add(country);
            await context.SaveChangesAsync();
            var dto = new GetCountryDto(
                country.CountryId,
                country.Name,
                country.ShortName,
                []
            );
            return Result<GetCountryDto>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<GetCountryDto>.Failure();
        }

    }

    public async Task<Result> DeleteCountryDto(int countryId)
    {
        try
        {
            var country = await context.Countries.FindAsync(countryId);
            if (country == null)
            {
                return Result.Failure(new Error("NotFound","Country is not find"));
            }
            context.Countries.Remove(country);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure();
        }
    }

    private async Task<bool> CountryExists(string name)
    {
        return await context.Countries.AnyAsync(c=> c.Name.ToLower().Trim() == name.ToLower().Trim());
    }
}
