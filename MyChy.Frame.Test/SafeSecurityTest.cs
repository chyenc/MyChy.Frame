using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Common;
using MyChy.Frame.Common.Helper;
using NUnit.Framework;

namespace MyChy.Frame.Test
{
    [TestFixture]
    public class SafeSecurityTest
    {
        private static readonly byte[] RgbIv = { 0x33, 0x34, 0x51, 120, 0x90, 0x3b, 0xcd, 0x1f };


        [Test]
        public void Run()
        {
            var sss = "asdfa";

           // var yy = SafeSecurity.Md5Encrypt(sss);
            var yy = SafeSecurity.Sha1(sss);
            sss = "我的";
            yy = SafeSecurity.Sha1(sss);
            sss = "我的";
           // yy = SafeSecurity.Sha1(sss, Encoding.GetEncoding("GB2312"));


            string str = System.Text.Encoding.UTF8.GetString(RgbIv);
            var ss = "";
            ss = "123";
            ss = StringHelper.StringQuantity(ss, 8);
            ss = "1232221342342";
            ss = StringHelper.StringQuantity(ss, 8);
        }
       // string str = System.Text.Encoding.Default.GetString ( byteArray );
    }
}
