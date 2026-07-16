using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Entities;

public class Country
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string ShortName { get; set; }
    
    public List<Hotel> Hotels { get; set; } = [];
}