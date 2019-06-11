using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
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
          var time2 = new DateTime(1970, 1, 1,8,0,0);
          var time= time2.AddMilliseconds(446400000000);

            
 time = time2.AddMilliseconds(1537128961000);


            var ss1=DayTimeHelper.ChangeTicks(446400000000);

            ss1 = DayTimeHelper.ChangeTicks(DateTime.Now.Ticks);

            DayTimeHelper.CheckTicks(1476429498);

            DayTimeHelper.CheckTicks(1515727650055);

           

                        DayTimeHelper.CheckTicks(DateTime.Now.Ticks);

            double s = (double)1476417251*1000;
            var log = DateTime.Now.Ticks - 621356256000000000;
            var s1s=log - s;


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


            var   resultxml = @"<xml>
  <appid><![CDATA[wx2421b1c4370ec43b]]></appid>
  <attach><![CDATA[支付测试]]></attach>
  <bank_type><![CDATA[CFT]]></bank_type>
  <fee_type><![CDATA[CNY]]></fee_type>
  <is_subscribe><![CDATA[Y]]></is_subscribe>
  <mch_id><![CDATA[10000100]]></mch_id>
  <nonce_str><![CDATA[5d2b6c2a8db53831f7eda20af46e531c]]></nonce_str>
  <openid><![CDATA[oUpF8uMEb4qRXf22hE3X68TekukE]]></openid>
  <out_trade_no><![CDATA[1409811653]]></out_trade_no>
  <result_code><![CDATA[SUCCESS]]></result_code>
  <return_code><![CDATA[SUCCESS]]></return_code>
  <sign><![CDATA[B552ED6B279343CB493C5DD0D78AB241]]></sign>
  <sub_mch_id><![CDATA[10000100]]></sub_mch_id>
  <time_end><![CDATA[20140903131540]]></time_end>
  <total_fee>1</total_fee>
  <trade_type><![CDATA[JSAPI]]></trade_type>
  <transaction_id><![CDATA[1004400740201409030005092168]]></transaction_id>
</xml>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(resultxml);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("xml").ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                var ss = xn.Name;
                ss = xn.InnerText;

            }
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
