using Microsoft.AspNetCore.Identity;

namespace BookingHotel.Data;

public class HotelAdmin
{
    public int Id { get; set; }

    public IdentityUser? User { get; set; }
    public required string UserId { get; set; }

    public Hotel? Hotel { get; set; }
    public int HotelId { get; set; }
}