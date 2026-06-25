using BookingHotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelsController(BookingHotelDbContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hotel>>> Index()
    {
        var hotels = await context
            .Hotels
           // .Include(h=> h.Country)
            .ToListAsync();
        return hotels;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Hotel>> GetHotel(int id)
    {
        var hotel = await context
            .Hotels 
            .Include(h => h.Country)
            .FirstOrDefaultAsync(h=> h.CountryId == id);
        if (hotel == null)
        {
            return NotFound();
        }

        return hotel;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, Hotel hotel)
    {
        if (id != hotel.Id)
        {
            return BadRequest();
        }

        context.Entry(hotel).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync();
        } 
        catch (DbUpdateConcurrencyException)
        {
            if (! await HotelExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent(); 
     }

    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
    {
        Console.WriteLine(hotel);
         context.Hotels.Add(hotel);
         await context.SaveChangesAsync();
         return CreatedAtAction("GetHotel",new { id = hotel.Id }, hotel);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> PostHotel(int id)
    {
        var hotel = await context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }
        context.Hotels.Remove(hotel);
        await context.SaveChangesAsync();
        return NoContent();
    }

    public async Task<bool> HotelExists(int id)
    {
        return await context.Hotels.AnyAsync(h=> h.Id == id);
    }


}