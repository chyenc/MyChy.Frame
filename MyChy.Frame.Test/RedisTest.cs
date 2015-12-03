using System;
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
            long xx = 0;
            RedisServer.StringSetCache("11", "asdfasdf");
            var ss = RedisServer.StringGetCache<string>("11");
            RedisServer.Remove("11");
            ss = RedisServer.StringGetCache<string>("11");
            var time = DateTime.Now;
            RedisServer.StringSetCache("21", time);
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
            xx = RedisServer.Increment("asdf11",100);
            xx = RedisServer.Increment("asdf11");
            xx = RedisServer.Increment("asdf11");
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
