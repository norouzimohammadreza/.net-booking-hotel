using BookingHotel.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private static List<Hotel> _hotels = new List<Hotel>
    {
        new Hotel
        {
            Id = 1, Name = "Azin", Address = "Gorgan", Rating = 3.7
        },
        new Hotel
        {
            Id = 2, Name = "MotelGhou", Address = "SalmanShahr", Rating = 4.3
        }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Hotel>> Get()
    {
        return Ok(_hotels );
    }
    
    [HttpGet("{id}")]
    public ActionResult<Hotel> Get(int id)
    {
        var hotel = _hotels.FirstOrDefault(h => h.Id == id);
        if (hotel == null)
        {
            return NotFound();
        }
        return hotel;
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var hotel = _hotels.FirstOrDefault(x => x.Id == id);
        if (hotel == null)
        {
            return NotFound(new { message = "Hotel not found" , data = id });
        }
        _hotels.Remove(hotel);
        return NoContent();
    }
    
    [HttpPost]
    public ActionResult<Hotel> Post([FromBody]Hotel newHotel)
    {
        if (_hotels.Any(h=>h.Id == newHotel.Id))
        {
            return BadRequest("Hotel already exists");
        }
        _hotels.Add(newHotel);
        return CreatedAtAction(nameof(Get), new { id = newHotel.Id }, newHotel);
    }
    
    [HttpPut("{id}")]
    public ActionResult Put(int id,[FromBody] Hotel updatedHotel)
    {
        var existingHotel = _hotels.FirstOrDefault(h => h.Id == id);
        if (existingHotel == null)
        {
            return NotFound();
        }

        existingHotel.Name = updatedHotel.Name;
        existingHotel.Address = updatedHotel.Address;
        existingHotel.Rating = updatedHotel.Rating;
        return NoContent();
    }
    
}