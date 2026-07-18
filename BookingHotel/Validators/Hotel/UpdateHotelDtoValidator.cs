using BookingHotel.DTOs.Hotel;
using FluentValidation;

namespace BookingHotel.Validators.Hotel;

public class UpdateHotelDtoValidator : AbstractValidator<UpdateHotelDto>
{
    public UpdateHotelDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(4, 60);

        RuleFor(x => x.Address)
            .NotEmpty()
            .Length(5, 120);

        RuleFor(x => x.Rating)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5);
        
        RuleFor(x => x.PerNightRating)
            .GreaterThan(0);
        
        RuleFor(x => x.CountryId)
            .GreaterThan(0);
    }
}