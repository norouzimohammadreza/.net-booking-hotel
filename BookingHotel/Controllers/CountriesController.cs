using BookingHotel.Data;
using BookingHotel.DTOs.Country;
using BookingHotel.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class CountriesController(BookingHotelDbContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var countries = await context
            .Countries
            .Select(c=> new GetCountriesDto(c.CountryId, c.Name,c.ShortName))
            .ToListAsync();
        return countries;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var country = await context
            .Countries
            .Where(c=> c.CountryId == id)
            //.Include(c => c.Hotels)
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
        if (country == null)
        {
            return NotFound();
        } 

        return country;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    { 
        if (id != countryDto.CountryId)
        {
            return BadRequest();
        }
        
        var country = await context.Countries.FindAsync(countryDto.CountryId);
        if (country == null)
        {
            return NotFound();
        }

        country.Name = countryDto.Name;

        context.Entry(country).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExistAsync(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
        var country = new Country
        {
            Name = countryDto.Name,
            ShortName = countryDto.ShortName
        };
        context.Countries.Add(country);
        await context.SaveChangesAsync();
        return CreatedAtAction("GetCountry", new { id = country.CountryId }, country);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }
        context.Countries.Remove(country);
        await context.SaveChangesAsync();
        return NoContent();
    } 

    private async Task<bool> CountryExistAsync(int id)
    {
        return await context.Countries.AnyAsync(c => c.CountryId == id);
    }

}