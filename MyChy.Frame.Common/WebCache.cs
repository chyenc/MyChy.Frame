using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyChy.Frame.Common
{
    /// <summary>
    /// WebCache 的摘要说明
    /// </summary>
    public static class WebCache
    {

        public static readonly bool IsCache = ConfigurationManager.AppSettings["CacheEnable"].To<bool>(false);
        private static readonly int CacheTime = ConfigurationManager.AppSettings["CacheTime"].To<int>(10);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
        {
            if (IsCache)
            {
                var objCache = HttpRuntime.Cache;
                var obj = objCache[cacheKey];
                return StringToObj<T>(obj);
            }
            else
            {
                return default(T);
            }

        }


        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            var time = DateTime.Now.AddMinutes((double)CacheTime);
            SetCache(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="minutes">时间</param>
        public static void SetCache(string cacheKey, object objObject, int minutes)
        {
            var time = DateTime.Now.AddMinutes((double)minutes);
            SetCache(cacheKey, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void SetCacheSeconds(string cacheKey, object objObject, int seconds)
        {
            var time = DateTime.Now.AddSeconds((double)seconds);
            SetCache(cacheKey, objObject, time);
        }



        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void Remove(string cacheKey)
        {
            if (!IsCache) return;
            var objCache = HttpRuntime.Cache;
            objCache.Remove(cacheKey);
        }


        #region

        /// <summary>
        /// 添加缓存 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="time"></param>
        private static void SetCache(string cacheKey, object objObject, DateTime time)
        {
            if (!IsCache) return;
            if (objObject == null) return;
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, time, TimeSpan.Zero);
        }


        /// <summary>
        /// 字符转化成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static T StringToObj<T>(object obj)
        {
            if (obj != null)
            {
                return (T)obj;
            }
            return default(T);
        }

        #endregion
    }
}
