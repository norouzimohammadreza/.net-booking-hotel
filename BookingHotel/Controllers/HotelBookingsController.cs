using BookingHotel.Contracts;
using BookingHotel.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers;

[ApiController]
[Route("api/hotels/{hotelId:int}/bookings")]
public class HotelBookingsController(IBookingService bookingService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBookingDto>>> GetBookings([FromRoute] int hotelId)
    {
        var result = await bookingService.GetBookingsForHotel(hotelId);
        return ToActionResult(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<GetBookingDto>> Create([FromRoute] int hotelId,[FromBody] CreateBookingDto createBookingDto){
        var result = await bookingService.CreateBooking(createBookingDto);
        return ToActionResult(result);
    }

    [HttpPut("{bookingId:int}")]
    public async Task<ActionResult<GetBookingDto>> Update(
        [FromRoute] int hotelId,
        [FromRoute] int bookingId,
        [FromBody] UpdateBookingDto updateBookingDto)
    {
        var result = await bookingService.UpdateBooking(hotelId, bookingId, updateBookingDto);
        return ToActionResult(result);
    }
} 