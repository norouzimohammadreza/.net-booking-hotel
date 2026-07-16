namespace BookingHotel.Entities;

public class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public double Rating { get; set; }
    
    public decimal PerNightRating { get; set; }
    
    public int CountryId { get; set; }
    
    public Country? Country { get; set; }
    
    public ICollection<HotelAdmin> Admins { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}
