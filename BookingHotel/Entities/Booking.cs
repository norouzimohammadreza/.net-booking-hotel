using BookingHotel.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookingHotel.Entities;


public class Booking
{
    public int Id { get; set; }
    
    public Hotel? Hotel { get; set; }
    public int HotelId { get; set; }
    
    public IdentityUser? User { get; set; }
    public required string UserId { get; set; }
    
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int Guests { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}
