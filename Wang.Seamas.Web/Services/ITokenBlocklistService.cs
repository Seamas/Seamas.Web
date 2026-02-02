namespace Wang.Seamas.Web.Services;

public interface ITokenBlocklistService
{
    Task BlocklistTokenAsync(string token, TimeSpan? expiry = null);
    Task<bool> IsTokenBlocklistedAsync(string token);
    Task RemoveExpiredTokensAsync();
}