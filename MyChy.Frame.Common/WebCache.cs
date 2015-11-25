﻿using System;
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

        static WebCache()
        {
            if (Config != null) return;
            Config = CfgConfig.Reader<WebCacheConfig>("config/WebCache.cfg", "Cache");
            if (Config?.CacheMinute==0)
            {
                Config = new WebCacheConfig { IsCache = false };
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
        {
            if (!Config.IsCache) return default(T);
            var objCache = HttpRuntime.Cache;
            var obj = objCache[cacheKey];
            return StringToObj<T>(obj);
        }


        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="cacheKey">KEY</param>
        /// <param name="objObject">数据</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            var time = DateTime.Now.AddMinutes(Config.CacheMinute);
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
            if (!Config.IsCache) return;
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
            if (!Config.IsCache) return;
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
