namespace Wang.Seamas.Web.DTOs;

public class AuthResult
{
    public bool IsAuthenticated { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = new List<string>();
    public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
    
    public int Version { get; set; }
}