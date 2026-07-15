using BookingHotel.Contracts;
using BookingHotel.DTOs.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers;

[ApiController]
[Route("api/hotels/{hotelId:int}/bookings")]
[Authorize]
public class HotelBookingsController(IBookingService bookingService) : BaseApiController
{
    [HttpGet("/admin")]
    [Authorize(Roles = "Hotel Admin, Admin")]
    public async Task<ActionResult<IEnumerable<GetBookingDto>>> GetBookings([FromRoute] int hotelId)
    {
        var result = await bookingService.GetBookingsForHotel(hotelId);
        return ToActionResult(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBookingDto>>> GetUserBookings([FromRoute] int hotelId)
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
    
    [HttpPut("{bookingId:int}/cancel")]
    public async Task<ActionResult > Cancel(
        [FromRoute] int hotelId,
        [FromRoute] int bookingId)
    {
        var result = await bookingService.CancelBooking(hotelId, bookingId);
        return ToActionResult(result);
    }
    
    [Authorize(Roles = "Hotel Admin, Admin")]
    [HttpPut("{bookingId:int}/admin/cancel")]
    public async Task<ActionResult > AdminCancel(
        [FromRoute] int hotelId,
        [FromRoute] int bookingId)
    {
        var result = await bookingService.AdminCancelBooking(hotelId, bookingId);
        return ToActionResult(result);
    }
    
    [HttpPut("{bookingId:int}/admin/confirm")]
    [Authorize(Roles = "Hotel Admin, Admin")]
    public async Task<ActionResult > AdminConfirm(
        [FromRoute] int hotelId,
        [FromRoute] int bookingId)
    {
        var result = await bookingService.AdminConfirmBooking(hotelId, bookingId);   
        return ToActionResult(result);
    }
} 