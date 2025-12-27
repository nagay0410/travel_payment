namespace Api.Common;

/// <summary>
/// 全ての API レスポンスで統一される共通の構造。
/// </summary>
/// <typeparam name="T">レスポンスデータの型</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse(T data, string? message = null)
    {
        Success = true;
        Data = data;
        Message = message;
    }

    public ApiResponse(List<string> errors, string? message = "Validation failed")
    {
        Success = false;
        Errors = errors;
        Message = message;
    }

    public static ApiResponse<T> CreateSuccess(T data, string? message = null) => new(data, message);
    public static ApiResponse<T> CreateFailure(List<string> errors, string? message = "Error occurred") => new(errors, message);
}
