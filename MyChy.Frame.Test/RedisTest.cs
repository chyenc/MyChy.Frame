using System;
using System.Globalization;
using MyChy.Frame.Common.Redis;
using NUnit.Framework;

namespace MyChy.Frame.Test
{
    [TestFixture]
    public class RedisTest
    {
        [Test]
        public void Run()
        {
            var key = "RegistrationServer_ShowPresentcount_" + DateTime.Now.Date.ToString("yyyy-MM-dd");
            var xx1 = RedisServer.StringGetCache<long>(key);

            long xx = 0;
            RedisServer.StringSetCache("11", "asdfasdf");
            var ss = RedisServer.StringGetCache<string>("11");
            //RedisServer.Remove("11");
           // ss = RedisServer.StringGetCache<string>("11");
          var time = DateTime.Now;

            var s = RedisServer.ExistsKey("11");

           
            RedisServer.StringDaySetCache("21", time);
            time = RedisServer.StringGetCache<DateTime>("21");

            var ts = new Tests
            {
                id = 1,
                Tilte = "123"
            };
            RedisServer.StringSetCache("31", ts);
            var ts1 = RedisServer.StringGetCache<Tests>("31");
            ts1.id = 2;
            RedisServer.StringSetCache("31", ts1);
            var ts2 = RedisServer.StringGetCache<Tests>("31");
           
                        //long xx = 0;

                        // xx = RedisServer.StringIncrement("asdf1", 10);
                        xx = RedisServer.Increment("asdf11");
            xx = RedisServer.Increment("asdf11");
            xx = RedisServer.Increment("asdf11");
            xx = RedisServer.StringGetCache<long>("asdf11");
            xx = RedisServer.Increment("asdf11",100);
            xx = RedisServer.Increment("asdf11");
            xx = RedisServer.StringGetCache<long>("asdf11");
            xx = RedisServer.Increment("asdf11");
          

        }

        [Test]
        public void SetTest()
        {
            const string key = "RedisTest_SetTest";
            long ss = 13810565157;
            string mobile = "13810565156";
            var ss1 = RedisServer.SetContainsCache(key, mobile);
            RedisServer.SetAddCache(key, mobile);
            ss1 = RedisServer.SetContainsCache(key, mobile);
            RedisServer.SetDelete(key, mobile);
            ss1 = RedisServer.SetContainsCache(key, mobile);

            if (!ss1)
            {
                RedisServer.SetDayAddCache(key, mobile,true);
                ss1 = RedisServer.SetDayContainsCache(key, mobile);

            }
         
            for (int i = 0; i < 100000; i++)
            {
                mobile = ss.ToString();
                ss1 = RedisServer.SetDayContainsCache(key, mobile);
                if (!ss1)
                {
                    RedisServer.SetDayAddCache(key, mobile);
                }
                ss = ss+ 1;
            }


           
        }


        [Test]
        public void HashTest()
        {



            string key = "Hash";
            string name =String.Empty;
            for (int i = 0; i < 10; i++)
            {
                name = Guid.NewGuid().ToString("N");
                RedisServer.HashAddCache(key, i.ToString(), name);
            }
            var xx = RedisServer.HashGetCache<string>(key, "1");
            RedisServer.HashDelete(key, "1");
            xx = RedisServer.HashGetCache<string>(key, "1");
            RedisServer.HashAddCache(key, "1", name);
            xx = RedisServer.HashGetCache<string>(key, "1");


            //for (int i = 0; i < 100; i++)
            //{
            //    name = Guid.NewGuid().ToString("N");
            //    RedisServer.HashAddCache(key, name, new Tests() {Tilte = name,id=i});
            //}
            //var zz = RedisServer.HashGetCache<Tests>(key, name);

            //for (int i = 0; i < 100; i++)
            //{
            //    name = Guid.NewGuid().ToString("N");
            //    RedisServer.HashAddCache(key,  i.ToString(), name);
            //}
            //var yy = RedisServer.HashGetCache<string>(key, "20");


        }

        public class Tests
        {
            public int id { get; set; }

            public string Tilte { get; set; }
        }

        //ceshi xiug 
        //
    }
}
