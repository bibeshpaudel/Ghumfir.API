using FluentValidation;
using Ghumfir.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghumfir.Application.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string?> ValidateMobile<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Mobile number is invalid.");
        }

        public static IRuleBuilderOptions<T, string?> ValidatePassword<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$")
                .WithMessage(
                "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }

        public static IRuleBuilderOptions<T, string?> ValidateRole<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must(role => role != null && Roles.AllRoles.Contains(role))
                .WithMessage("Invalid Role");
        }

        public static IRuleBuilderOptions<T, string?> ValidateFullname<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Length(2, 100).WithMessage("Full name must be between 2 and 100 characters.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Full name must only contain letters and spaces.");
        }
    }
}
