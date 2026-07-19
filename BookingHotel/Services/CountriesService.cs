using BookingHotel.Data;
using BookingHotel.DTOs.Country;
using BookingHotel.Contracts;
using BookingHotel.DTOs.Hotel;
using BookingHotel.DTOs.Pagination;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Results;
using BookingHotel.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Services;

public class CountriesService(BookingHotelDbContext context) : ICountriesService
{
    public async Task<Result<PagedResult<GetCountriesDto>>> GetCountries([FromQuery]PaginationQuery pagination)
    {
        var totalCount = await context.Countries
            .CountAsync();
        
        var countries = await context
            .Countries
            .Select(c=> new GetCountriesDto(c.Id, c.Name,c.ShortName))
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(); 
        
        var result = new PagedResult<GetCountriesDto>
        {
            Items = countries,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
        return Result<PagedResult<GetCountriesDto>>.Success(result);
    }

    public async Task<Result<GetCountryDto>> GetCountry(int id)
    {
        var country = await context
            .Countries
            .Where(c=> c.Id == id)
            .Select(c=> new GetCountryDto(
                c.Id,
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
            if (id != countryDto.Id)
            {
                return Result.Failure(new Error("Validation","Country id does not match"));
            }
            var country = await context.Countries.FindAsync(countryDto.Id);
            if (country == null)
            {
                return Result.Failure(new Error("NotFound","Country is not find"));
            }
            
            var duplicateName = await context.Countries.AnyAsync(c=> c.Id != id && c.Name == countryDto.Name);

            if (duplicateName)
            {
                return Result.Failure(new Error("Duplicate","Country name already exists"));
            }

            country.Update(countryDto.Name, countryDto.ShortName);

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
            var country = new Country(countryDto.Name, countryDto.ShortName);
            context.Countries.Add(country);
            await context.SaveChangesAsync();
            var dto = new GetCountryDto(
                country.Id,
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
