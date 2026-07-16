using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Entities;

public class Country
{
    public int CountryId { get; set; }
    [Required]
    [MaxLength(50)]
    [MinLength(5)]
    public string Name { get; set; }
    [Required]
    [MaxLength(4)]
    [MinLength(2)]
    public string ShortName { get; set; }
    public List<Hotel> Hotels { get; set; } = [];
}