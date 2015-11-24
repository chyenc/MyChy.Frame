using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Common.Helper;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace MyChy.Frame.Common.Redis
{
    internal class SerializeHelper
    {
        /// <summary>
        /// 类序列化成字符
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjToString(object obj)
        {
            string result = string.Empty;

            if (obj == null) return result;
            var ty = obj.GetType();
            var ty1 = ty.BaseType;
            if (ty1 != null && ty1.Name == "ValueType")
            {
                result = JsonConvert.SerializeObject(obj);
            }
            else
            {
                if ((ty.Name == "String") || (ty.Name == "StringBuilder"))
                {
                    result = obj.ToString();
                }
                else
                {
                     result = JsonConvert.SerializeObject(obj);
                }

            }

            return result;

        }

        /// <summary>
        /// 字符转化成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ty"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T StringToObj<T>(string value)
        {
            object result;
            try
            {
                var ty = typeof(T);
                var ty1 = ty.BaseType;
                if ((ty1 != null) && (ty1.Name == "ValueType"))
                {
                    result = JsonConvert.DeserializeObject(value, ty);
                }
                else
                {
                    if ((ty.Name == "String") || (ty.Name == "StringBuilder"))
                    {
                        if (ty.Name == "String")
                        {
                            result = value.ToString();
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            sb.Append(value);
                            result = sb;
                        }
                    }
                    else
                    {
                        if ((value.Substring(0, 1) == "{") || (value.Substring(0, 2) == "[{"))
                        {
                            result =JsonConvert.DeserializeObject<T>(value);
                        }
                        else
                        {
                            var memStream2 = new MemoryStream(Convert.FromBase64String(value));
                            var formatter = new BinaryFormatter();
                            result = formatter.Deserialize(memStream2);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
                result = null;
            }
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }
    }
}
