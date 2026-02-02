
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Wang.Seamas.Web.Services.Model;

namespace Wang.Seamas.Web.Services.Impl;

public class TokenService : ITokenService
{
    private readonly CustomAuthOptions _options;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptions<CustomAuthOptions> options, ILogger<TokenService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public string GenerateToken(AuthResult authResult)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecretKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authResult.UserId.ToString()),
            new Claim(ClaimTypes.Name, authResult.Username),
            new Claim(ClaimTypes.Email, authResult.Email),
            new Claim("AuthenticatedAt", DateTime.UtcNow.ToString("O")),
            new Claim(nameof(AuthResult.Version), authResult.Version.ToString())
        };

        // 添加角色声明
        foreach (var role in authResult.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 添加自定义声明
        foreach (var claim in authResult.Claims)
        {
            claims.Add(new Claim(claim.Key, claim.Value));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.TokenExpiry),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _options.Issuer,
            Audience = _options.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public AuthResult? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.SecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // 严格检查过期时间
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            
            int.TryParse(principal.FindFirst(nameof(AuthResult.Version))?.Value, out var version);
            
            // 从 Claims 创建 AuthResult
            var authResult = new AuthResult
            {
                IsAuthenticated = true,
                UserId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                Version = version
            };

            // 提取角色
            authResult.Roles = principal.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            // 提取自定义声明（排除标准声明）
             var standardClaimTypes = new[]
            {
                ClaimTypes.NameIdentifier,
                ClaimTypes.Name,
                ClaimTypes.Email,
                ClaimTypes.Role,
                "AuthenticatedAt"
            };

            authResult.Claims = principal.Claims
                .Where(c => !standardClaimTypes.Contains(c.Type))
                .ToDictionary(c => c.Type, c => c.Value);

            return authResult;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return null;
        }
    }
}