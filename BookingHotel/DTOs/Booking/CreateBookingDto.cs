namespace BookingHotel.DTOs.Booking;

public class CreateBookingDto
{
   public int HotelId { get; set; }
   public DateOnly CheckIn  { get; set; }
   public DateOnly CheckOut  { get; set; }
   public int Guests { get; set; }
}

