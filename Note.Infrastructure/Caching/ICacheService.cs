namespace Note.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key);
        Task Set<T>(string key, T value, TimeSpan expiration);
        Task Remove(string key);
    }
}
