using Wang.Seamas.Web.DTOs;

namespace Wang.Seamas.Web.Interfaces;

public interface ITokenService
{
    string GenerateToken(AuthResult authResult);
    AuthResult? ValidateToken(string token);
}
