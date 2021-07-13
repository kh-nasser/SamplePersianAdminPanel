using Common.DataModel.Domain.Logger;

namespace DataAccess.Logger
{
    public class LoggerDao
    {
        public long Create(LogDto log)
        {
            using (var db = new PersianAdminPanelEntities())
            {
                var dataset = db.Set<tblLog>();
                var data = new tblLog()
                {
                    Callsite = log.Callsite,
                    CreatedAt = log.CreatedAt,
                    Duration = log.Duration,
                    Exception = log.Exception,
                    LogLevel = log.Level.ToString(),
                    Message = log.Message
                };

                dataset.Add(data);
                db.SaveChanges();
                var result = data.Id;
                return result;
            }
        }
    }
}