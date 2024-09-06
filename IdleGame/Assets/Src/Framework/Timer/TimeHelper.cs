using System;

namespace GameFrameWork
{
    public class TimeHelper
    {
        public static long GetUTCNowTick()
        {
            return DateTime.UtcNow.Ticks - TimeDefine.UTCTimeTick;
        }
        
        /// <summary>
        /// 根据服务器时间返回UTC时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetUTCTime(long time)
        {
            DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            dt_1970 = dt_1970.AddTicks(time);
            return dt_1970;
        }
    }
}