using Microsoft.AspNetCore.Identity;

namespace BookingHotel.Entities;


public class HotelAdmin
{
    public int Id { get; private set; }

    public IdentityUser? User { get; private set; }
    public string UserId { get; private set; }

    public Hotel? Hotel { get; private set; }
    public int HotelId { get; private set; }
    
    private HotelAdmin(){}

    public HotelAdmin(int hotelId, string userId)
    {
        HotelId = hotelId;
        UserId = userId;
    }
}