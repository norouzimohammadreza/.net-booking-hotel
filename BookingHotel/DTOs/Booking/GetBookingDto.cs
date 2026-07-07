namespace BookingHotel.DTOs.Booking;

public record GetBookingDto
(
    int Id,
    int HotelId,
    string HotelName,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Guest,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);