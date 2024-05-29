namespace TeraAuthApi.Application.Wrapper;

public class ApiServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static ApiServiceResponse<T> SuccessResponse(T data, string message = null)
    {
        return new ApiServiceResponse<T> { Success = true, Message = message, Data = data };
    }

    public static ApiServiceResponse<T> FailureResponse(string message = null)
    {
        return new ApiServiceResponse<T> { Success = false, Message = message };
    }
}