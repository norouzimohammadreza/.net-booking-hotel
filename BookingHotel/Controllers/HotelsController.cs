using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Hotel;
using BookingHotel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HotelsController(IHotelsService hotelsService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelsDto>>> Index()
    {
        var hotels = await hotelsService.GetHotels();
        return ToActionResult(hotels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsService.GetHotel(id);
        return ToActionResult(hotel);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
       var result = await hotelsService.UpdateHotel(id, hotelDto);
       return ToActionResult(result);
     }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotelDto)
    {
        var result = await hotelsService.CreateHotel(hotelDto);
        if (!result.IsSuccess)
        {
         return MapErrorsToResponse(result.Errors);
        }
        return CreatedAtAction("GetHotel", new { id = result.Value!.Id }, result.Value);

    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteHotel(int id)
    {
      var result =  await hotelsService.DeleteHotel(id);
      return ToActionResult(result);
    }
}