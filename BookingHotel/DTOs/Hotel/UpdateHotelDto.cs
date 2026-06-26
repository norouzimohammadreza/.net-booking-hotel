using System.ComponentModel.DataAnnotations;

namespace BookingHotel.DTOs.Hotel;

public class UpdateHotelDto
{
    [Required]
    public int Id { get; set; }
}