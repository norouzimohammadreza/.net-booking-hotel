namespace BookingHotel.DTOs.User;

public record GetUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

}