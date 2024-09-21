namespace Ghumfir.Application.DTOs;

public class ApiResponse<T> where T : class?
{
    public static ApiResult<T> Success(string code, string message, T data)
    {
        return new ApiResult<T>()
        {
            ResponseCode = code,
            ResponseMessage = message,
            Data = data
        };
    }

    public static ApiResult<T> Success(string message, T data)
    {
        return new ApiResult<T>()
        {
            ResponseCode = "0",
            ResponseMessage = message,
            Data = data
        };
    }

    public static ApiResult<T> Success(T data)
    {
        return new ApiResult<T>()
        {
            ResponseCode = "0",
            ResponseMessage = "success",
            Data = data
        };
    }

    public static ApiResult<T> Success()
    {
        return new ApiResult<T>()
        {
            ResponseCode = "0",
            ResponseMessage = "success",
            Data = default
        };
    }


    public static ApiResult<T> Failed(string code, string message, T data)
    {
        return new ApiResult<T>()
        {
            ResponseCode = code,
            ResponseMessage = message,
            Data = data
        };
    }
    public static ApiResult<T> Failed(string message)
    {
        return new ApiResult<T>()
        {
            ResponseCode = "1",
            ResponseMessage = message,
            Data = default
        };
    }
    public static ApiResult<T> Failed(string message, T data)
    {
        return new ApiResult<T>()
        {
            ResponseCode = "1",
            ResponseMessage = message,
            Data = data
        };
    }

    public static ApiResult<T> Failed(T data)
    {
        return new ApiResult<T>()
        {
            ResponseCode = "1",
            ResponseMessage = "failed",
            Data = data
        };
    }
    public static ApiResult<T> Failed()
    {
        return new ApiResult<T>()
        {
            ResponseCode = "1",
            ResponseMessage = "failed",
            Data = default
        };
    }
}