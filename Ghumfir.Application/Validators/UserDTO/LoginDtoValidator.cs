using FluentValidation;
using Ghumfir.Application.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghumfir.Application.Validators.UserDTO
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Mobile)
                .NotEmpty().WithMessage("Mobile is required.")
                .ValidateMobile();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .ValidatePassword();
        }
    }
}
