using Oracle.ManagedDataAccess.Client;
using Polly;

namespace Note.Infrastructure.Common.Retry
{
    public static class PollyHelper
    {
        public static IAsyncPolicy CreateOracleRetryPolicy()
        {
            return Policy
                .Handle<OracleException>(OracleExceptionHelper.IsTransient)
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2));
        }
    }
}
