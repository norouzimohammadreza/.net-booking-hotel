using BookingHotel.DTOs.Country;
using FluentValidation;

namespace BookingHotel.Validators.Country;

public class UpdateCountryDtoValidator :AbstractValidator<UpdateCountryDto>
{
    public UpdateCountryDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(5, 50);

        RuleFor(x => x.ShortName)
            .NotEmpty()
            .Length(2, 4);
    }
}