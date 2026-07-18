using BookingHotel.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookingHotel.Entities;


public class Booking
{
    public int Id { get; private set; }
    
    public Hotel? Hotel { get; private set; }
    public int HotelId { get; private set; }
    
    public IdentityUser? User { get; private set; }
    public string UserId { get; private set; }
    
    public DateOnly CheckIn { get; private set; }
    public DateOnly CheckOut { get; private set; }
    public int Guests { get; private set; }
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    
    private Booking() { }
    
    public Booking(
        int hotelId,
        string userId,
        DateOnly checkIn,
        DateOnly checkOut,
        int guests,
        decimal perNightPrice)
    {
        HotelId = hotelId;
        UserId = userId;

        SetStayInfo(checkIn, checkOut, guests, perNightPrice);

        CreatedAt = DateTime.UtcNow;
        Status = BookingStatus.Pending;
    }
    
    public void Update(
        DateOnly checkIn,
        DateOnly checkOut,
        int guests,
        decimal perNightPrice)
    {
        SetStayInfo(checkIn, checkOut, guests, perNightPrice);
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetStayInfo(
        DateOnly checkIn,
        DateOnly checkOut,
        int guests,
        decimal perNightPrice)
    {
        CheckIn = checkIn;
        CheckOut = checkOut;
        Guests = guests;

        var nights = checkOut.DayNumber - checkIn.DayNumber;
        TotalPrice = nights * perNightPrice;
    }
    
    public void Cancel()
    {
        Status = BookingStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Confirm()
    {
        Status = BookingStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

}
