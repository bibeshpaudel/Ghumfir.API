using FluentValidation;
using Ghumfir.Application.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghumfir.Application.Validators.UserDTO
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Password is required.")
                .ValidatePassword();

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .ValidatePassword();

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Confirm new password is required.")
                .Equal(x => x.NewPassword).WithMessage("New password and Confirm new password must match.");
        }
    }
}
