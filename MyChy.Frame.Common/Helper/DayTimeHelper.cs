using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Helper
{
   public  static class DayTimeHelper
    {
        /// <summary>
        /// 计算星期几
        /// </summary>
        /// <param name="oneWeek"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static DateTime OneDayOfWeek(int oneWeek, DayOfWeek week)
        {
            var weekday = oneWeek * 7;
            var days = (int)week;
            if (days == 0) days = 7;
            var dayweek = DateTime.Now.DayOfWeek;
            var days1 = (int)dayweek;
            if (days1 == 0) days1 = 7;
            int addday = weekday + days - days1;
            return DateTime.Now.AddDays(addday).Date;
        }
    }
}
