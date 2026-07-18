using BookingHotel.DTOs.User;
using FluentValidation;

namespace BookingHotel.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(80)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(20);

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => role == "Admin" || role == "User" || role == "Hotel Admin")
            .WithMessage("The role is wrong.");
    }
    
}