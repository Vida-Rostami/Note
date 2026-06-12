using StackExchange.Redis;
using System.Text.Json;

namespace Note.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();

        }

        public async Task<T?> Get<T>(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (data.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(data!);
        }

        public async Task Set<T>(string key, T value, TimeSpan expiration)
        {
            var json = JsonSerializer.Serialize(value);

            await _database.StringSetAsync(
                key,
                json,
                expiration);
        }

        public async Task Remove(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task<int> GetVersion(string key)
        {
            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                await _database.StringSetAsync(key, 1);
                return 1;
            }

            return int.Parse(value!);
        }

        public async Task IncrementVersion(string key)
        {
            await _database.StringIncrementAsync(key);
        }

    }
}
