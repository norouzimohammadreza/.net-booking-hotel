using System.ComponentModel.DataAnnotations;

namespace BookingHotel.DTOs.User;

public class CreateUserDto
{
    [Required]
    [EmailAddress]
    [MaxLength(80)]
    public string Email { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }

    public string Role { get; set; } = "User";  
}