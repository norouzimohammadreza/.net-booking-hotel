using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.DTOs.Hotel;
using BookingHotel.Results;
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
        return ToActionResult(hotels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsService.GetHotel(id);
        return ToActionResult(hotel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
       var result = await hotelsService.UpdateHotel(id, hotelDto);
       return ToActionResult(result);
     }

    [HttpPost]
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
    public async Task<ActionResult> DeleteHotel(int id)
    {
      var result =  await hotelsService.DeleteHotel(id);
      return ToActionResult(result);
    }
    
    private ActionResult<T> ToActionResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : MapErrorsToResponse(result.Errors);
    
    private ActionResult ToActionResult(Result result) =>
        result.IsSuccess ? NoContent() : MapErrorsToResponse(result.Errors);

    private ActionResult MapErrorsToResponse(Error[] errors)
    {
        if (errors == null || errors.Length == 0)
        {
            return Problem();
        }
        var e = errors[0];
        return e.Code switch
        {
            "NotFound" => NotFound(e.Description),
            "BadRequest" => BadRequest(e.Description),
            "Validation" => BadRequest(e.Description),
            _ => Conflict(e.Description)
        };
    }
}