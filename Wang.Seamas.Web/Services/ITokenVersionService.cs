namespace Wang.Seamas.Web.Services;

public interface ITokenVersionService
{
    Task<int> GetCurrentVersionAsync(int userId);
    Task<int> IncrementVersionAsync(int userId);
    Task<bool> ValidateTokenVersionAsync(int userId, int tokenVersion);
}