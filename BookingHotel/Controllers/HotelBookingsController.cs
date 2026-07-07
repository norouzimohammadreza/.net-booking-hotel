using BookingHotel.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers;

[ApiController]
[Route("api/hotels/{hotelId}/bookings")]
public class HotelBookingsController : Controller
{
    [HttpGet]
    public async Task<ActionResult<GetBookingDto>> GetBookings([FromRoute] int hotelId)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult<GetBookingDto>> Create([FromRoute] int hotelId,[FromBody] CreateBookingDto createBookingDto){
        return Ok();
    }
} 