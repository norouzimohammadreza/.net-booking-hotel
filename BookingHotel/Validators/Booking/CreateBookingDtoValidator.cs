using BookingHotel.DTOs.Booking;
using FluentValidation;

namespace BookingHotel.Validators.Booking;

public class CreateBookingDtoValidator:AbstractValidator<CreateBookingDto>
{
    public  CreateBookingDtoValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0);

        RuleFor(x => x.Guests)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12);

        RuleFor(x => x.CheckIn)
            .LessThan(x => x.CheckOut)
            .WithMessage("Check-in date must be before check-out date.");
    }
}