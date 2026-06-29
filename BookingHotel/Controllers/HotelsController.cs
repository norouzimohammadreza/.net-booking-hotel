using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelsDto>>> Index()
    {
        var hotels = await hotelsService.GetHotels();
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsService.GetHotel(id);
        if (hotel == null)
        {
            return NotFound();
        }

        return Ok(hotel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest();
        }

        await hotelsService.UpdateHotel(id, hotelDto);

        return NoContent(); 
     }

    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
    {
        var hotel = await hotelsService.CreateHotel(hotelDto);
         return CreatedAtAction("GetHotel",new { id = hotel.Id }, hotel);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> PostHotel(int id)
    {
        await hotelsService.DeleteHotel(id);
        return NoContent();
    }


}