namespace Ghumfir.Application.DTOs;

public class ApiResult<T>
{
    public required string ResponseCode { get; set; }
    public required string ResponseMessage { get; set; }
    public T? Data { get; set; }
}