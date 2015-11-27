using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyChy.Frame.Test
{
    [TestFixture]
    public class EnterpriseLibraryTest
    {
        [Test]
        public void Run()
        {
            string openid = "";
            var wehy10000 = new SqlTest.Wehy10000Sql();

            var count = wehy10000.Run(openid);

        }
    }
}
