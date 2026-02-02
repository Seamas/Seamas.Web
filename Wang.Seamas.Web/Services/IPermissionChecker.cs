namespace Wang.Seamas.Web.Services;

public interface IPermissionChecker
{
    // 检查用户是否有权访问某个 API（URL）
    Task<bool> IsUserAllowedToAccessApiAsync(int userId, string url);
    
}