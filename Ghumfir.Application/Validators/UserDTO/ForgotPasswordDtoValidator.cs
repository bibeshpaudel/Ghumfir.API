using FluentValidation;
using Ghumfir.Application.DTOs.UserDTO;

namespace Ghumfir.Application.Validators.UserDTO;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile is required.")
            .ValidateMobile();
    }
}

public class VerifyForgotPasswordDtoValidator : AbstractValidator<VerifyForgotPasswordDto>
{
    public VerifyForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile is required.")
            .ValidateMobile();
        
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .When(x => x.Code.ToString().Length != 6).WithMessage("Code must be 6 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New Password is required.")
            .ValidatePassword();
    }
}