using Oracle.ManagedDataAccess.Client;

namespace Note.Infrastructure.Common.Retry
{
    public static class OracleExceptionHelper
    {
        public static bool IsTransient(OracleException ex)
        {
            return ex.Number is 12170   // timeout
                or 12541              // listener down
                or 12560;             // network issue
        }
    }
}
