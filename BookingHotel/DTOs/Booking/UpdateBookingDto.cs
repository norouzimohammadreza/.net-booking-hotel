namespace BookingHotel.DTOs.Booking;

public class UpdateBookingDto
{
  public DateOnly CheckIn { get; set; }
  public DateOnly CheckOut { get; set; }
  public int Guests { get; set; }
}

