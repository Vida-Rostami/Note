using Polly;

namespace Note.Infrastructure.Common
{
    public static class PollyHelper
    {
        public static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int retryCount = 3, int delaySecond = 2)
        {
            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(retryCount, attemp => TimeSpan.FromSeconds(delaySecond));
            return await retryPolicy.ExecuteAsync(action);
        }
    }
}
