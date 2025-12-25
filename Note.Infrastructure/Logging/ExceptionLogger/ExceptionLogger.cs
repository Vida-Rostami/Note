using Dapper;
using Microsoft.Extensions.Options;
using Note.Domain;
using Note.Domain.Log;
using Oracle.ManagedDataAccess.Client;

namespace Note.Infrastructure.Log.ExceptionLoggerService
{
    public class ExceptionLogger : IExceptionLogger
    {
        private DatabaseSettings _option;
        public ExceptionLogger(IOptions<DatabaseSettings> options)
        {
            _option = options.Value;
        }
        public async Task LogException(ExceptionLog exceptionLog)
        {
            using var connection = new OracleConnection(_option.OracleConnection);
            await connection.OpenAsync();

            var sql = @"
                        INSERT INTO TblExceptionLog
                        (action, exceptionmessage, stacktrace, innerexception, request, userid, createdatetime, traceId)
                        VALUES
                        (:Action, :ExceptionMessage, :StackTrace, :InnerException, :Request, :UserId, SYSTIMESTAMP, :traceId)
                    ";
            await connection.ExecuteAsync(sql, exceptionLog);
        }
    }
}
