using Ghumfir.Application.DTOs;
using Ghumfir.Application.DTOs.UserDTO;

namespace Ghumfir.Application.Contracts;

public interface IUser
{
    Task<ApiResult<string?>> RegisterUser(RegisterUserDto request);
    Task<ApiResult<LoginResponse>> LoginUser(LoginDto request);
    Task<ApiResult<LoginResponse>> RefreshToken(RefreshTokenDto request);
    Task<ApiResult<string?>> ChangePassword(ChangePasswordDto request);
}