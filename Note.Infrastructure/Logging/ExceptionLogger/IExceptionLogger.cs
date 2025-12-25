using Note.Domain.Log;

namespace Note.Infrastructure.Log.ExceptionLoggerService
{
    public interface IExceptionLogger
    {
        Task LogException(ExceptionLog exceptionLog);
    }
}