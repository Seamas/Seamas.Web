namespace Wang.Seamas.Web.Common.Exceptions;

public class AuthException(string message, Exception? innerException = null) : Exception(message, innerException);