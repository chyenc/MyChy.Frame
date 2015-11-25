using System;
using MyChy.Frame.Common.Helper;
using StackExchange.Redis;

namespace MyChy.Frame.Common.Redis
{
    public class RedisServer
    {
        private static readonly RedisConfig Config = null;

        private static bool _isCacheError = false;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = 
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(Config.Connect));

        private static ConnectionMultiplexer Redis => LazyConnection.Value;

        static RedisServer()
        {
            if (Config != null) return;
            Config = CfgConfig.Reader<RedisConfig>("config/redis.cfg", "redis");
            if (string.IsNullOrEmpty(Config?.Connect))
            {
                Config = new RedisConfig {IsCache = false};
            }
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
                _isCacheError = true;
            }
            return null;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
        {
            if (Config.IsCache && !_isCacheError)
            {
                var redisdb = Redis.GetDatabase();
                if (_isCacheError) return default(T);
                var obj = redisdb.StringGet(Config.Name +cacheKey);
                return SerializeHelper.StringToObj<T>(obj);
            }
            else
            {
                return default(T);
            }

        }


        #region 删除缓存

        public static void Remove(string cacheKey)
        {
            if (!Config.IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            if (_isCacheError) return;
            redisdb.KeyDelete(Config.Name + cacheKey);
        }

        public static void RemoveAsync(string cacheKey)
        {
            if (!Config.IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            if (_isCacheError) return;
            redisdb.KeyDeleteAsync(Config.Name + cacheKey);

        }

        #endregion

        #region 同步增加缓存

        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            var time = DateTime.Now.AddSeconds(Config.CacheSeconds);
            SetCache(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void SetCache(string cacheKey, object objObject, int seconds)
        {
            var time = DateTime.Now.AddSeconds(seconds);
            SetCache(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="time"></param>
        public static void SetCache(string cacheKey, object objObject, DateTime time)
        {
            if (!Config.IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (_isCacheError) return;
            redisdb.StringSet(Config.Name + cacheKey, obj, ts);
        }

        #endregion


        #region 异步增加缓存

        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        public static void SetCacheAsync(string cacheKey, object objObject)
        {
            var time = DateTime.Now.AddSeconds(Config.CacheSeconds);
            SetCacheAsync(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void SetCacheAsync(string cacheKey, object objObject, double seconds)
        {
            var time = DateTime.Now.AddSeconds(seconds);
            SetCacheAsync(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="time"></param>
        public static void SetCacheAsync(string cacheKey, object objObject, DateTime time)
        {
            if (!Config.IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (_isCacheError) return;
            redisdb.StringSetAsync(Config.Name + cacheKey, obj, ts);
        }

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
            if (!Config.IsCache || _isCacheError) return -1;
            var redisdb = Redis.GetDatabase();
            return _isCacheError ? -1 : redisdb.StringIncrement(Config.Name + key, cardinal);
        }

        /// <summary>
        /// 原子加计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Increment(string key)
        {
            if (!Config.IsCache || _isCacheError) return -1;
            var redisdb = Redis.GetDatabase();
            return _isCacheError ? -1 : redisdb.StringIncrement(Config.Name + key);
        }

        /// <summary>
        /// 原子减计数器 第一次赋值后cardinal 管用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Decrement(string key, long cardinal)
        {
            if (!Config.IsCache || _isCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return _isCacheError ? 0 : redisdb.StringDecrement(Config.Name + key, cardinal);
        }

        /// <summary>
        /// 原子减计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Decrement(string key)
        {
            if (!Config.IsCache || _isCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return _isCacheError ? 0 : redisdb.StringDecrement(Config.Name + key);
        }

        #endregion

    }
}
