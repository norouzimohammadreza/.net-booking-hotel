using BookingHotel.DTOs.Booking;
using BookingHotel.Results;

namespace BookingHotel.Contracts;

public interface IBookingService
{
     Task<Result<IEnumerable<GetBookingDto>>> GetBookingsForHotel(int hotelId);
     Task<Result<GetBookingDto>> CreateBooking(CreateBookingDto createBookingDto);
}