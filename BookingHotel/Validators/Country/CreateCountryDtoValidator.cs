using BookingHotel.DTOs.Country;
using FluentValidation;

namespace BookingHotel.Validators.Country;

public class CreateCountryDtoValidator:AbstractValidator<CreateCountryDto>
{
    public CreateCountryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(5, 50);

        RuleFor(x => x.ShortName)
            .NotEmpty()
            .Length(2, 4);
    }
}