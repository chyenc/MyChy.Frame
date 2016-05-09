using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyChy.Frame.Common.Helper
{
    public static class CookieHelper
    {
        private static readonly string Des3Key = WebConfig.AppSettingsName<string>("CookieHelperKey", "dtvb^*3e");

        static CookieHelper()
        {

        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domainname">如果为空，则设置当前域名</param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static bool Set(string name, string value, string domainname,int minute)
        {
            if (value == null) return false;
            //设定cookie 过期时间.
            var dtExpiry = DateTime.Now.AddMinutes(minute);
            var httpCookie = HttpContext.Current.Response.Cookies[name];
            if (httpCookie == null) return false;
            HttpCookie sessionCookie = null;

            //对 SessionId 进行备份.
            if (HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
            {
                string sesssionId = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString();
                sessionCookie = new HttpCookie("ASP.NET_SessionId") { Value = sesssionId };
            }

            httpCookie.Expires = dtExpiry;
            //设定cookie 域名.
            if (domainname.Length == 0)
            {
                var domain = string.Empty;
                if (HttpContext.Current.Request.Params["HTTP_HOST"] != null)
                {
                    //domain = "www.elong.com";
                    domain = HttpContext.Current.Request.Params["HTTP_HOST"].ToString();
                }

                if (domain.IndexOf(".", System.StringComparison.Ordinal) > -1)
                {
                    var index = domain.IndexOf(".", System.StringComparison.Ordinal);
                    domain = domain.Substring(index + 1);
                    httpCookie.Domain = domain;

                }
            }
            else
            {
                httpCookie.Domain = domainname;
            }

            httpCookie.Value = value;

            //如果cookie总数超过20 个, 重写ASP.NET_SessionId, 以防Session 丢失.
            if (HttpContext.Current.Request.Cookies.Count <= 20 || sessionCookie == null) return true;
            if (sessionCookie.Value == string.Empty) return true;
            HttpContext.Current.Response.Cookies.Remove("ASP.NET_SessionId");
            HttpContext.Current.Response.Cookies.Add(sessionCookie);
            return true;
        }

        public static bool Set(string name, string value, string domainname)
        {
           return Set(name, value, domainname, 60*24*360);
        }


        public static bool Set(string name, object value, string domainname)
        {
            var vals = SerializeHelper.ObjToString(value);
            return Set(name, vals, domainname);

        }

        /// <summary> 
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            string cookyval;
            try
            {
                if (HttpContext.Current.Request.Cookies[key] == null)
                {
                    return "";
                }
                cookyval = HttpContext.Current.Request.Cookies[key].Value;
            }
            catch
            {
                return "";
            }
            return cookyval;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T Get<T>(string key, T def)
        {
            var vals = Get(key);
            return SerializeHelper.StringToObj<T>(vals, def);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            var vals = Get(key);
            return SerializeHelper.StringToObj<T>(vals);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domainname"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static bool Set3Des(string name, string value, string domainname, int minute)
        {
            value = SafeSecurity.EncryptDes(value, Des3Key);
            return Set(name, value, domainname, minute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get3Des(string key)
        {
            var cookyval = Get(key);
            if (!string.IsNullOrEmpty(key))
            {
                cookyval = SafeSecurity.DecryptDes(cookyval, Des3Key);
            }
            return cookyval;
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Set(key, "", "",0);
        }
    }
}
