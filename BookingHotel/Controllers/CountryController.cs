using BookingHotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class CountryController(BookingHotelDbContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        var countries = await context
            .Countries
             //.Include(c => c.Hotels)
            .ToListAsync();
        return countries;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        var country = await context
            .Countries
            .Include(c => c.Hotels)
            .FirstOrDefaultAsync(c=> c.CountryId == id);
        if (country == null)
        {
            return NotFound();
        } 

        return country;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, Country country)
    { 
        if (id != country.CountryId)
        {
            return BadRequest();
        }

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
    public async Task<ActionResult<Country>> PostCountry(Country country)
    {
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