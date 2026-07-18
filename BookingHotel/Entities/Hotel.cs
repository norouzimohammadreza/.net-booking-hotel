namespace BookingHotel.Entities;

public class Hotel
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Address { get; private set; }

    public double Rating { get; private set; }

    public decimal PerNightRating { get; private set; }

    public int CountryId { get; private set; }

    public Country? Country { get; private set; }

    public ICollection<HotelAdmin> Admins { get; private set; } = [];

    public ICollection<Booking> Bookings { get; private set; } = [];
    
    public Hotel(
        string name,
        string address,
        double rating,
        decimal perNightRating,
        int countryId)
    {
        Name = name;
        Address = address;
        Rating = rating;
        PerNightRating = perNightRating;
        CountryId = countryId;
    }
    
    public void Update(
        string name,
        string address,
        double rating,
        decimal perNightRating,
        int countryId)
    {
        Name = name;
        Address = address;
        Rating = rating;
        PerNightRating = perNightRating;
        CountryId = countryId;
    }
}
