using System.ComponentModel.DataAnnotations;

namespace BookingHotel.DTOs.Hotel;

public class CreateHotelDto
{
    [Required]
    [MaxLength(60)]
    [MinLength(4)]
    public string Name { get; set; }

    [Required]
    [MaxLength(120)]
    [MinLength(5)]
    public string Address { get; set; }

    [Required] 
    [Range(0, 5)] 
    public double Rating { get; set; }

    public int CountryId { get; set; }
}