namespace AuthService.Application.Interfaces.Services
{
    public interface IRedisTokenStore
    {
        public Task StoreRefreshTokenAsync(string userId, string token);
        public Task<bool> IsTokenValidAsync(string userId, string token);
    }
}
