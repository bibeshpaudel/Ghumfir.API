using FluentValidation;
using Ghumfir.Application.DTOs.UserDTO;

namespace Ghumfir.Application.Validators.UserDTO;

public partial class RegisterDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile is required.")
            .ValidateMobile();

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .ValidateFullname();

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .ValidatePassword();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.Password).WithMessage("Password and Confirm password must match.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .ValidateRole();

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email is invalid.");
    }
}