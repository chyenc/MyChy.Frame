using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Helper
{
    public static class WebExtends
    {
        #region 转换成URL参数

        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + value + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + value + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        public static string ToQueryString(this IEnumerable<object> list, string key)
        {
            var sb = new StringBuilder();
            foreach (var val in list)
            {
                if (val != null)
                {
                    sb.Append(key + "=" + Uri.EscapeDataString(val.ToString()) + "&");
                }
            }
            return sb.ToString().TrimEnd('&').Substring(key.Length + 1);
        }


        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToOrderString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            var dictionarys = dictionary.OrderBy(x => x.Key).ToList();
            foreach (var key in dictionarys.Where(key => key.Key != null))
            {
                sb.Append(key.Key + "=" + key.Value + "&");
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToOrderString(this IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            var dictionarys = dictionary.OrderBy(x => x.Key).ToList();
            foreach (var key in dictionarys.Where(key => key.Key != null))
            {
                sb.Append(key.Key + "=" + key.Value + "&");
            }
            return sb.ToString().TrimEnd('&');
        }


        #endregion

        #region 转换成XML

        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToXmlString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.AppendFormat("<{0}>{1}</{0}>", key,value);
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        #endregion

    }
}
