using FluentValidation;
using Ghumfir.Application.Contracts;
using Ghumfir.Application.DTOs.UserDTO;
using Ghumfir.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Asp.Versioning;

namespace Ghumfir.API.Controllers;
[ApiVersion(1)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    IUser user,
    IValidator<RegisterUserDto> registerValidator,
    IValidator<LoginDto> loginValidator,
    IValidator<RefreshTokenDto> refreshValidator,
    IValidator<ChangePasswordDto> changePwdValidator,
    IValidator<ForgotPasswordDto> forgotPwdValidator,
    IValidator<VerifyForgotPasswordDto> verifyForgotPwdValidator,
    IUserAccessor userAccessor) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto request)
    {
        var validationResult = await loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await user.LoginUser(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<string?>> RegisterUser([FromBody] RegisterUserDto request)
    {
        var validationResult = await registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await user.RegisterUser(request);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string?>> RefreshToken([FromBody] RefreshTokenDto request)
    {
        var validationResult = await refreshValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await user.RefreshToken(request);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<string?>> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var validationResult = await changePwdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await user.ChangePassword(request);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordDto request)
    {
        var validationResult = await forgotPwdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await user.ForgotPassword(request);
        return Ok(result);
    }

    [HttpPost("verify-forgot-password")]
    public async Task<ActionResult<string?>> VerifyForgotPassword([FromBody] VerifyForgotPasswordDto request)
    {
        var validationResult = await verifyForgotPwdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await user.VerifyForgotPassword(request);
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Customer}")]
    [HttpPost("test")]
    public Task<ActionResult<string?>> Test()
    {
        return Task.FromResult<ActionResult<string?>>(
            Ok($"{userAccessor.GetMobile()}, {userAccessor.GetUserId()}, {userAccessor.GetFullname()}"));
    }
}