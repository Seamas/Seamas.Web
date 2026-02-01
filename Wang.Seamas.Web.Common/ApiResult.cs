namespace Wang.Seamas.Web.Common
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public long Timestamp => DateTime.UtcNow.Ticks;


        public static ApiResult Ok() => new() { Success = true };
        public static ApiResult Fail(string message) => new() { Success = false, Message = message };
    }
    
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        
        public long Timestamp => DateTime.UtcNow.Ticks;

        public static ApiResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static ApiResult<T> Fail(string message) => new() { Success = false, Message = message };
    }
}