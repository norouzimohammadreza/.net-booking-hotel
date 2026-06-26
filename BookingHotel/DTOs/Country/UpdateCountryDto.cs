using System.ComponentModel.DataAnnotations;

namespace BookingHotel.DTOs.Country;

public class UpdateCountryDto
{
    [Required]
    public int CountryId { get; set; }
}