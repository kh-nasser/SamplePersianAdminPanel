using System;

namespace Common.DataModel.Domain.Logger
{

    public class LogDto
    {
        public long Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public string Callsite { set; get; }
        public string Message { set; get; }
        public long? Duration { set; get; }
        public string Exception { set; get; }
        public Level Level { set; get; }
    }
}
