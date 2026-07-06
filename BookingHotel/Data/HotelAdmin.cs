using Microsoft.AspNetCore.Identity;

namespace BookingHotel.Data;

public class HotelAdmin
{
    public int Id{ get; set; }
    
    public IdentityUser? User { get; set; }
    public required string UserId { get; set; } 

    public Hotel? Hotel { get; set; }
    public int HotelId { get; set; } 
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int Guests { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }     
    public string Status = "Pending, Confirmed, Cancelled";
}