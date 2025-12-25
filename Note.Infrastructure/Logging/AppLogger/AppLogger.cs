using Dapper;
using Microsoft.Extensions.Options;
using Note.Domain;
using Note.Domain.Log;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Infrastructure.Log.AppLogger
{
    public class AppLogger : IAppLogger 
    {
        private readonly DatabaseSettings _options;
        
        public AppLogger(IOptions<DatabaseSettings> options)
        {
            _options = options.Value;
        }
        public async Task Log(AppLog appLog)
        {
            using var connection = new OracleConnection(_options.OracleConnection); 
            connection.Open();
            var sql = @"
                INSERT INTO TBLAPPLOG
                (action, request, response, statuscode, userid, method, path, createdatetime, durationms)
                VALUES
                (:Action, :Request, :Response, :StatusCode, :UserId, :Method, :Path, SYSTIMESTAMP, :DurationMs)
            ";
            await connection.ExecuteAsync(sql, appLog);
        }
    }
}
