namespace Wang.Seamas.Web.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }
    string UserName { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
}