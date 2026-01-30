namespace Wang.Seamas.Web.Common;

/// <summary>
/// 验证异常
/// </summary>
public class ValidateException(string message, Exception? innerException = null) : Exception(message, innerException);