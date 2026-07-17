using System.ComponentModel.DataAnnotations;

namespace BookingHotel.DTOs.Country;

public class CreateCountryDto
{
    public string Name { get; set; }
    
    public string ShortName { get; set; }
}