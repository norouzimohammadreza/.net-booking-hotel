using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Data;

public class Hotel
{
    public int Id { get; set; }
    [Required]
    [MaxLength(60)]
    [MinLength(4)]
    public string Name { get; set; }
    [Required]
    [MaxLength(120)]
    [MinLength(5)]
    public string Address { get; set; }
    [Required]
    [Range(0,5)]
    public double Rating { get; set; }
    
    public decimal PerNightRating { get; set; }
    
    public int CountryId { get; set; }
    
    public Country? Country { get; set; }
    
    public ICollection<HotelAdmin> Admins { get; set; } = [];
}
