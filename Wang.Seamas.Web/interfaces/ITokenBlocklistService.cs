namespace Wang.Seamas.Web.Interfaces;

public interface ITokenBlocklistService
{
    Task BlocklistTokenAsync(string token, TimeSpan? expiry = null);
    Task<bool> IsTokenBlocklistedAsync(string token);
    Task RemoveExpiredTokensAsync();
}