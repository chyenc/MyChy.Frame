using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MyChy.Frame.Common.Helper
{
    public class StringHelper
    {
        private static readonly char[] Constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// 生成随机数字码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int length)
        {
            var guid = Guid.NewGuid();
            var newRandom = new System.Text.StringBuilder(length);
            var rd = new Random(guid.GetHashCode());
            for (var i = 0; i < length; i++)
            {
                newRandom.Append(Constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        public static string GenerateRandomCode(int num)
        {
            var guid = Guid.NewGuid();
            var random = new Random(guid.GetHashCode());
            var newRandom = new System.Text.StringBuilder(num);
            for (var i = 0; i < num; i++)
            {
                var number = random.Next();

                char code;
                if (number % 2 == 0)
                    code = (char)('1' + (char)(number % 9));
                else
                    code = (char)('A' + (char)(number % 26));

                newRandom.Append(code);
            }
            return newRandom.ToString();

        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        #region XML反序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T DeserializeXml<T>(string xml)
        {
            try
            {
                var type = typeof(T);
                var name = type.Name;
                var sb = new StringBuilder(xml);
                sb.Replace("<xml>", "<" + name + ">");
                sb.Replace("</xml>", "</" + name + ">");
                object obj = null;
                using (var sr = new StringReader(sb.ToString()))
                {
                    var xmldes = new XmlSerializer(type);
                    obj = xmldes.Deserialize(sr);
                }
                if (obj != null)
                {
                    return (T)obj;
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }
            return default(T);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SerializerXml(object obj)
        {
            var str = "";
            try
            {
                var type = obj.GetType();
                var stream = new MemoryStream();
                var xml = new XmlSerializer(type);
                //序列化对象
                xml.Serialize(stream, obj);
                stream.Position = 0;
                var sr = new StreamReader(stream);
                str = sr.ReadToEnd();

                sr.Dispose();
                stream.Dispose();
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }
            return str;
        }

        #endregion

        /// <summary>
        /// 制定字符串数量
        /// </summary>
        /// <returns></returns>
        public static string StringQuantity(string value, int length)
        {
            if (value.Length != length)
            {
                value = value.Length < length ? value.PadLeft(length, '0') : value.Substring(0, length);
            }
            return value;
        }
    }
}
