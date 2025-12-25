using Note.Domain.Log;

namespace Note.Infrastructure.Log.AppLogger
{
    public interface IAppLogger
    {
        Task Log(AppLog  appLog);
    }
}
