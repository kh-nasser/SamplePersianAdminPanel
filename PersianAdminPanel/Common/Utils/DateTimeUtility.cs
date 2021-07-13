using System;

namespace Common.Utils
{
    public class DateTimeUtility
    {
        public long EpochUtcNow()
        {
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }
    }
}