namespace Wang.Seamas.Web.Common.Dtos;

public class ApiResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public int Code { get; set; } = 200;
    public long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();


    public static ApiResult Ok() => new() { Success = true };
    public static ApiResult Fail(string message, int code = 500 ) => new() { Success = false, Message = message, Code = code };
}

public class ApiResult<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    
    public int Code { get; set; } = 200;
    public T? Data { get; set; }
    
    public long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public static ApiResult<T> Ok(T? data ) => new() { Success = true, Data = data };
    public static ApiResult<T> Fail(string message, int code = 500 ) => new() { Success = false, Message = message, Code = code };
}
