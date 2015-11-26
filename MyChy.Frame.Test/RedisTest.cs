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
            RedisServer.SetCache("1", "asdfasdf");
            var ss = RedisServer.GetCache<string>("1");
            RedisServer.Remove("1");
            ss = RedisServer.GetCache<string>("1");
            var time = DateTime.Now;
            RedisServer.SetCache("2", time);
            time = RedisServer.GetCache<DateTime>("2");

            var ts = new Tests
            {
                id = 1,
                Tilte = "123"
            };
            RedisServer.SetCache("3", ts);
            var ts1 = RedisServer.GetCache<Tests>("3");
            ts1.id = 2;
            RedisServer.SetCache("3", ts1);
            var ts2 = RedisServer.GetCache<Tests>("3");



           // xx = RedisServer.StringIncrement("asdf1", 10);
            xx = RedisServer.Increment("asdf1");
            xx = RedisServer.Increment("asdf1");
            xx = RedisServer.Increment("asdf1");
            xx = RedisServer.Increment("asdf1",100);
            xx = RedisServer.Increment("asdf1");
            xx = RedisServer.Increment("asdf1");
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
