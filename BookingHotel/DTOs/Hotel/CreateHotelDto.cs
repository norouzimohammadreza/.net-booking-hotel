namespace BookingHotel.DTOs.Hotel;

public class CreateHotelDto
{
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public double Rating { get; set; }

    public int CountryId { get; set; }
}