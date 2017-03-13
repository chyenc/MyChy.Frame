using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyChy.Frame.Common.Helper;
using MyChy.Frame.Common.Model;

namespace MyChy.Frame.Common
{
    /// <summary>
    /// WebCache 的摘要说明
    /// </summary>
    public static class WebCache
    {
        private static readonly WebCacheConfig Config = null;

        public static readonly bool IsCache;

        static WebCache()
        {
            if (Config != null) return;
            Config = CfgConfig.Reader<WebCacheConfig>("config/WebCache.cfg", "Cache");
            if (Config==null||Config.CacheMinute==0)
            {
                Config = new WebCacheConfig { IsCache = false };
            }
            IsCache = Config.IsCache;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetCache<T>(string key,T def)
        {
            if (!Config.IsCache) return default(T);
            var objCache = HttpRuntime.Cache;
            var obj = objCache[key];
            return obj.To<T>(def);

            
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetCache<T>(string key)
        {
            if (!Config.IsCache) return default(T);
            var objCache = HttpRuntime.Cache;
            var obj = objCache[key];
            return obj.To<T>();

        }


        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void SetCache(string key, object objObject)
        {
            var time = DateTime.Now.AddMinutes(Config.CacheMinute);
            SetCache(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="minutes">时间</param>
        public static void SetCache(string key, object objObject, int minutes)
        {
            var time = DateTime.Now.AddMinutes((double)minutes);
            SetCache(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void SetCacheSeconds(string key, object objObject, int seconds)
        {
            var time = DateTime.Now.AddSeconds((double)seconds);
            SetCache(key, objObject, time);
        }



        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (!Config.IsCache) return;
            var objCache = HttpRuntime.Cache;
            objCache.Remove(key);
        }


        #region

        /// <summary>
        /// 添加缓存 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <param name="time"></param>
        private static void SetCache(string key, object objObject, DateTime time)
        {
            if (!Config.IsCache) return;
            if (objObject == null) return;
            var objCache = HttpRuntime.Cache;
            objCache.Insert(key, objObject, null, time, TimeSpan.Zero);
        }


        #endregion
    }
}
