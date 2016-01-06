using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Common.Helper;
using NUnit.Framework;

namespace MyChy.Frame.Test
{
    [TestFixture]
    public class DayTimeHelperTest
    {
        [Test]
        public void Run()
        {
            int ss = DayTimeHelper.SecondsRemainingDay();
            DateTime da = DateTime.Now.AddSeconds(ss);

            long tokin = DateTime.Now.Ticks;
            int xx = DayTimeHelper.CalculatingDifferenceTicksSecond(tokin);
            xx = DayTimeHelper.CalculatingDifferenceTicksMillisecond(tokin);

        }
    }
}
