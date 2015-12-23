using System;
using MyChy.Frame.Common.Helper;
using StackExchange.Redis;

namespace MyChy.Frame.Common.Redis
{
    public class RedisServer
    {
        private static readonly RedisConfig Config = null;

        public static bool IsCacheError = false;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(Config.Connect));

        private static ConnectionMultiplexer Redis => LazyConnection.Value;

        static RedisServer()
        {
            if (Config != null) return;
            Config = CfgConfig.Reader<RedisConfig>("config/redis.cfg", "redis");
            if (string.IsNullOrEmpty(Config?.Connect))
            {
                Config = new RedisConfig { IsCache = false };
            }
            if (!Config.IsCache)
            {
                IsCacheError = true;
            }
            try
            {
                var connect = ConnectionMultiplexer.Connect(Config.Connect);
                var res = connect.ClientName;
            }
            catch (Exception exception)
            {
                Config.IsCache = false;
                LogHelper.Log(exception);
                IsCacheError = true;
            }
            //finally
            //{
            //    Config.IsCache = false;
            //    IsCacheError = true;
            //}

        }

        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    return ConnectionMultiplexer.Connect(constr);
        //});

        //public static ConnectionMultiplexer redis
        //{
        //    get
        //    {
        //        return lazyConnection.Value;
        //    }
        //}

        /// <summary>
        /// 获得缓存接口类
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            try
            {
                return Redis.GetDatabase();
            }
            catch (Exception exception)
            {
                LogHelper.Log(exception);
                IsCacheError = true;
            }
            
            return null;
        }





        #region 删除缓存

        public static void Remove(string key)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            redisdb.KeyDelete(Config.Name + key);
        }

        public static void RemoveAsync(string key)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            redisdb.KeyDeleteAsync(Config.Name + key);

        }

        /// <summary>
        /// key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistsKey(string key)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = Redis.GetDatabase();
            if (IsCacheError) return false;
            return redisdb.KeyExists(Config.Name + key);
        }

        #endregion

        #region String缓存

        /// <summary>
        /// 获取String缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGetCache<T>(string key)
        {
            if (!Config.IsCache || IsCacheError) return default(T);
            var redisdb = Redis.GetDatabase();
            if (IsCacheError) return default(T);
            var obj = redisdb.StringGet(Config.Name + key);
            return SerializeHelper.StringToObj<T>(obj);
        }


        #region 同步增加 String缓存

        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringSetCache(string key, object objObject)
        {
            var time = DateTime.Now.AddSeconds(Config.CacheSeconds);
            StringSetCache(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void StringSetCache(string key, object objObject, int seconds)
        {
            var time = DateTime.Now.AddSeconds(seconds);
            StringSetCache(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="time"></param>
        public static void StringSetCache(string key, object objObject, DateTime time)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (IsCacheError) return;
            redisdb.StringSet(Config.Name + key, obj, ts);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringSetCacheDay(string key, object objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(DateTime.Now.AddDays(1).Date).Duration();
            if (IsCacheError) return;
            redisdb.StringSet(Config.Name + key, obj, ts);
        }

        #endregion


        #region 异步增加 String缓存

        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringSetCacheAsync(string key, object objObject)
        {
            var time = DateTime.Now.AddSeconds(Config.CacheSeconds);
            StringSetCacheAsync(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void StringSetCacheAsync(string key, object objObject, double seconds)
        {
            var time = DateTime.Now.AddSeconds(seconds);
            StringSetCacheAsync(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="time"></param>
        public static void StringSetCacheAsync(string key, object objObject, DateTime time)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (IsCacheError) return;
            redisdb.StringSetAsync(Config.Name + key, obj, ts);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringSetCacheDayAsync(string key, object objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(DateTime.Now.AddDays(1).Date).Duration();
            if (IsCacheError) return;
            redisdb.StringSetAsync(Config.Name + key, obj, ts);
        }

        #endregion


        #endregion


        #region 原子计数器

        /// <summary>
        /// 原子加计数器 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Increment(string key, long cardinal)
        {
            if (!Config.IsCache || IsCacheError) return -1;
            var redisdb = Redis.GetDatabase();
     
            return IsCacheError ? -1 : redisdb.StringIncrement(Config.Name + key, cardinal);
        }

        /// <summary>
        /// 原子加计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Increment(string key)
        {
            if (!Config.IsCache || IsCacheError) return -1;
            var redisdb = Redis.GetDatabase();
            return IsCacheError ? -1 : redisdb.StringIncrement(Config.Name + key);
        }

        /// <summary>
        /// 原子减计数器 第一次赋值后cardinal 管用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Decrement(string key, long cardinal)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return IsCacheError ? 0 : redisdb.StringDecrement(Config.Name + key, cardinal);
        }

        /// <summary>
        /// 原子减计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Decrement(string key)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return IsCacheError ? 0 : redisdb.StringDecrement(Config.Name + key);
        }

        #endregion

        #region Set 无序存储数组

        /// <summary>
        /// Set列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        public static void SetAddCache(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            redisdb.SetAdd(Config.Name + key, obj);
        }

        /// <summary>
        /// Set列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        public static void SetAddCacheAsync(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            redisdb.SetAddAsync(Config.Name + key, obj);
        }

        /// <summary>
        /// 判断Set 列表是否存在数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <returns></returns>
        public static bool SetContainsCache(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            return redisdb.SetContains(Config.Name + key, objObject);
        }

        /// <summary>
        /// Set列表增加数据 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        public static void SetAddCacheDay(string key, string objObject, bool isDelYesterday = false)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            if (isDelYesterday)
            {
                var keyday = key + DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
                var val = ExistsKey(keyday + "str");
                if (!val)
                {
                    StringSetCacheDayAsync(keyday + "str", "1");
                    Remove(keyday);
                }
            }
            key = key + DateTime.Now.Date.ToString("yyyy-MM-dd");

            SetAddCache(key, objObject);
        }

        /// <summary>
        /// Set列表增加数据 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        public static void SetAddCacheDayAsync(string key, string objObject, bool isDelYesterday = false)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            if (isDelYesterday)
            {
                var keyday = key + DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
                var val = ExistsKey(keyday + "str");
                if (!val)
                {
                    StringSetCacheDayAsync(keyday + "str", "1");
                    Remove(keyday);
                }
            }
            key = key + DateTime.Now.Date.ToString("yyyy-MM-dd");

            SetAddCacheAsync(key, objObject);
        }


        /// <summary>
        /// 判断Set 列表是否存在数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <returns></returns>
        public static bool SetContainsCacheDay(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            key = key + DateTime.Now.Date.ToString("yyyy-MM-dd");
            return redisdb.SetContains(Config.Name + key, objObject);
        }

        #endregion

        #region List 有序存储数据

        ///// <summary>
        ///// Set列表增加数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="objObject">数据</param>
        //public static void ListAddCache(string key, string objObject)
        //{
        //    if (!Config.IsCache || IsCacheError) return;
        //    var redisdb = GetDatabase();
        //    var obj = SerializeHelper.ObjToString(objObject);
        //    if (IsCacheError) return;
        //    redisdb.ListInsertAfter(Config.Name + key, obj);


        #endregion

        #region hash 

#endregion

    }
}
