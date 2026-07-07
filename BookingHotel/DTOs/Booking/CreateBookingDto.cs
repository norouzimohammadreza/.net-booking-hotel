namespace BookingHotel.DTOs.Booking;

public record CreateBookingDto(
    int HotelId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Guest
    );