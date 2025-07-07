namespace UXComex.Domain.DTOs.Shared;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public ApiResponse()
    {

    }

    public ApiResponse(bool success)
    {
        Success = success;
    }

    public ApiResponse(bool success, T data)
    {
        Success = success;
        Data = data;
    }

    public ApiResponse(bool success, List<string> errors)
    {
        Success = success;
        Errors = errors;
    }

    public ApiResponse(bool success, string error)
    {
        Success = success;
        Errors = new()
        {
            error
        };
    }
}