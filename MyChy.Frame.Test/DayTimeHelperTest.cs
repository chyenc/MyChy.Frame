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
    public class DayTimeHelperTest
    {
        [Test]
        public void Run()
        {
            //int ss = DayTimeHelper.SecondsRemainingDay();
            //DateTime da = DateTime.Now.AddSeconds(ss);

            //long tokin = DateTime.Now.Ticks;
            //int xx = DayTimeHelper.CalculatingDifferenceTicksSecond(tokin);
            //xx = DayTimeHelper.CalculatingDifferenceTicksMillisecond(tokin);

                 //默认密钥向量

            //var str =
            //    "appid=wxb7b2b05063232049&attach=测试&body=汇源果汁-微店商品&mch_id=10013670&notify_url=http://hy80299997.21move.net/we_pay1/&openid=o8KB3uEwEIye3ceovZRej1uG1rMM&out_trade_no=636087526758546138&spbill_create_ip=0.0.0.0&time_expire=20160907110541&time_start=20160906100541&total_fee=1&trade_type=JSAPI&key=830CF7DC0978411EBB252FA5E8FB33CB";
            //var ss = SafeSecurity.Md5Encrypt(str);


            //str =
            //    "appid=wxb7b2b05063232049&attach=测试&key=830CF7DC0978411EBB252FA5E8FB33CB";

            //ss = SafeSecurity.Md5Encrypt(str);


          var   resultxml = @"<xml><return_code><![CDATA[SUCCESS]]></return_code>
<return_msg><![CDATA[OK]]></return_msg>
<appid><![CDATA[wxb7b2b05063232049]]></appid>
<mch_id><![CDATA[10013670]]></mch_id>
<nonce_str><![CDATA[jKj8ziE0D8AVpnpC]]></nonce_str>
<sign><![CDATA[7EA8427B4A489BDC3F5265D244DB2DD2]]></sign>
<result_code><![CDATA[SUCCESS]]></result_code>
<prepay_id><![CDATA[wx20160906101859bcc6a266810014246892]]></prepay_id>
<trade_type><![CDATA[JSAPI]]></trade_type>
</xml>";

            var payReturn = StringHelper.DeserializeXml<PayReturnModel>(resultxml);

        }
    }

    public class PayReturnModel
    {
        public string return_code { get; set; }

        public string return_msg { get; set; }

        public string appid { get; set; }

        public string mch_id { get; set; }

        public string nonce_str { get; set; }

        public string sign { get; set; }

        public string prepay_id { get; set; }

        public string trade_type { get; set; }
    }
}
