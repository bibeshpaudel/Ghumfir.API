using FluentValidation;
using Ghumfir.Application.DTOs.UserDTO;

namespace Ghumfir.Application.Validators.UserDTO
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(dto => dto.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
