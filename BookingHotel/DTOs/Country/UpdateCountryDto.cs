using System.ComponentModel.DataAnnotations;

namespace BookingHotel.DTOs.Country;

public class UpdateCountryDto
{
    [Required]
    public int CountryId { get; set; }
    [Required]
    [MaxLength(50)]
    [MinLength(5)]
    public string Name { get; set; }
    [Required]
    [MaxLength(4)]
    [MinLength(2)]
    public string ShortName { get; set; }
}