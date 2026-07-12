namespace BookingHotel.DTOs.Booking;

public record UpdateBookingDto(
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Guests
    );