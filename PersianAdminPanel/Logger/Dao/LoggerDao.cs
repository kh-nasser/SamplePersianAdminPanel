using Common.DataModel.Domain.Logger;

namespace Logger
{
    public class LoggerDao
    {
        DataAccess.Logger.LoggerDao _loggerDao = new DataAccess.Logger.LoggerDao();

        public long Create(LogDto log)
        {
            return _loggerDao.Create(log);
        }
    }
}
