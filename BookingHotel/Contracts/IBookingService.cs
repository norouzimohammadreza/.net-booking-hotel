using BookingHotel.DTOs.Booking;
using BookingHotel.Results;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Contracts;

public interface IBookingService
{
     Task<Result<IEnumerable<GetBookingDto>>> GetBookingsForHotel(int hotelId);
     Task<Result<IEnumerable<GetBookingDto>>> GetUserBookings(int hotelId);
     Task<Result<GetBookingDto>> CreateBooking(CreateBookingDto createBookingDto);
     Task<Result<GetBookingDto>> UpdateBooking(        
         [FromRoute] int hotelId,
         [FromRoute] int bookingId,
         [FromBody] UpdateBookingDto updateBookingDto
         );
     
         Task<Result> CancelBooking([FromRoute] int hotelId, [FromRoute] int bookingId);
         Task<Result> AdminCancelBooking([FromRoute] int hotelId, [FromRoute] int bookingId);
         Task<Result> AdminConfirmBooking([FromRoute] int hotelId, [FromRoute] int bookingId);
}