using System;
using MyChy.Frame.Common.Helper;
using StackExchange.Redis;

namespace MyChy.Frame.Common.Redis
{
    public class RedisServer
    {
        private static readonly bool IsCache = WebConfig.AppSettingsName<bool>("RedisIsCache", false);

        private static readonly string RedisName = WebConfig.AppSettingsName<string>("RedisName", "MyChy");

        private static readonly int CacheSeconds = WebConfig.AppSettingsName<int>("RedisCacheSeconds", 600);

        private static bool _isCacheError = false;

        private static readonly string Constr = WebConfig.AppSettingsName<string>("RedisConnect");

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(Constr));

        private static ConnectionMultiplexer Redis => LazyConnection.Value;

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
            if (IsCache && !_isCacheError)
            {
                var redisdb = Redis.GetDatabase();
                if (_isCacheError) return default(T);
                var obj = redisdb.StringGet(RedisName+cacheKey);
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
            if (!IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            if (_isCacheError) return;
            redisdb.KeyDelete(RedisName + cacheKey);
        }

        public static void RemoveAsync(string cacheKey)
        {
            if (!IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            if (_isCacheError) return;
            redisdb.KeyDeleteAsync(RedisName + cacheKey);

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
            var time = DateTime.Now.AddSeconds(CacheSeconds);
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
            var time = DateTime.Now.AddSeconds((double)seconds);
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
            if (!IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (_isCacheError) return;
            redisdb.StringSet(RedisName + cacheKey, obj, ts);
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
            var time = DateTime.Now.AddSeconds(CacheSeconds);
            SetCacheAsync(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void SetCacheAsync(string cacheKey, object objObject, int seconds)
        {
            var time = DateTime.Now.AddSeconds((double)seconds);
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
            if (!IsCache || _isCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (_isCacheError) return;
            redisdb.StringSetAsync(RedisName + cacheKey, obj, ts);
        }

        #endregion


        #region 原子计数器

        /// <summary>
        /// 原子加计数器 第一次赋值后cardinal 管用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Increment(string key, long cardinal)
        {
            if (!IsCache || _isCacheError) return -1;
            var redisdb = Redis.GetDatabase();
            return _isCacheError ? -1 : redisdb.StringIncrement(RedisName + key, cardinal);
        }

        /// <summary>
        /// 原子减计数器 第一次赋值后cardinal 管用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Decrement(string key, long cardinal)
        {
            if (!IsCache || _isCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return _isCacheError ? 0 : redisdb.StringDecrement(RedisName + key, cardinal);
        }

        #endregion

    }
}
