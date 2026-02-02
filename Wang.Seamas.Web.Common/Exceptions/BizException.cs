namespace Wang.Seamas.Web.Common.Exceptions;

public class BizException(string message, int code = 400, Exception? innerException = null )
    : Exception(message, innerException)
{
    public int Code { get; } = code;
}