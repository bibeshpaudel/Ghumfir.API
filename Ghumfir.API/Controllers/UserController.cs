using FluentValidation;
using Ghumfir.Application.Contracts;
using Ghumfir.Application.DTOs.UserDTO;
using Ghumfir.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ghumfir.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(IUser user, IValidator<RegisterUserDto> registerValidator, IValidator<LoginDto> loginValidator, IValidator<RefreshTokenDto> refreshValidator, IValidator<ChangePasswordDto> changePwdValidator, IValidator<ForgotPasswordDto> forgotPwdValidator, IValidator<VerifyForgotPasswordDto> verifyForgotPwdValidator, IUserAccessor userAccessor) : ControllerBase
{
    private readonly IUser _user = user;
    private readonly IValidator<RegisterUserDto> _registerValidator = registerValidator;
    private readonly IValidator<LoginDto> _loginValidator = loginValidator;
    private readonly IValidator<ChangePasswordDto> _changePwdValidator = changePwdValidator;
    private readonly IValidator<ForgotPasswordDto> _forgotPwdValidator = forgotPwdValidator;
    private readonly IValidator<VerifyForgotPasswordDto> _verifyForgotPwdValidator = verifyForgotPwdValidator;
    private readonly IUserAccessor _userAccessor = userAccessor;

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await _user.LoginUser(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<string?>> RegisterUser([FromBody] RegisterUserDto request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await _user.RegisterUser(request);
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

        var result = await _user.RefreshToken(request);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<string?>> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var validationResult = await _changePwdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }

        var result = await _user.ChangePassword(request);
        return Ok(result);
    }
    
    [HttpPost("forgot-password")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordDto request)
    {
        var validationResult = await _forgotPwdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }
        
        var result = await _user.ForgotPassword(request);
        return Ok(result);
    }
    
    [HttpPost("verify-forgot-password")]
    public async Task<ActionResult<string?>> VerifyForgotPassword([FromBody] VerifyForgotPasswordDto request)
    {
        var validationResult = await _verifyForgotPwdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }
        
        var result = await _user.VerifyForgotPassword(request);
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Customer}")]
    [HttpPost("test")]
    public Task<ActionResult<string?>> Test()
    {
        return Task.FromResult<ActionResult<string?>>(Ok($"{_userAccessor.GetMobile()}, {_userAccessor.GetUserId()}, {_userAccessor.GetFullname()}"));
    }
}