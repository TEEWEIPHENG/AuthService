using AuthService.Application.Interfaces.Services;
using StackExchange.Redis;

namespace AuthService.Infrastructure.Services
{
    public class RedisTokenStore : IRedisTokenStore
    {
        private readonly IDatabase _db;
        public RedisTokenStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task StoreRefreshTokenAsync(string userId, string token)
        {
            await _db.StringSetAsync($"refresh:{userId}", token, TimeSpan.FromDays(7));
        }

        public async Task<bool> IsTokenValidAsync(string userId, string token)
        {
            var stored = await _db.StringGetAsync($"refresh:{userId}");
            return stored == token;
        }
    }
}
