using Wang.Seamas.Web.Services.Model;

namespace Wang.Seamas.Web.Services;

public interface ITokenService
{
    string GenerateToken(AuthResult authResult);
    AuthResult? ValidateToken(string token);
}
