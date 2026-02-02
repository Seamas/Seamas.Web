namespace Wang.Seamas.Web.Services.Model;

public class CustomAuthOptions
{
    public string TokenHeaderName { get; set; } = "Authorization";
    public string TokenPrefix { get; set; } = "Bearer ";
    public TimeSpan TokenExpiry { get; set; } = TimeSpan.FromHours(2);
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}