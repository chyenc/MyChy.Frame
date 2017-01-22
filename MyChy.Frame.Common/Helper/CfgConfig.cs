using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpConfig;

namespace MyChy.Frame.Common.Helper
{
    public class CfgConfig
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">CFG/INI</param>
        /// <param name="section">模块</param>
        /// <returns></returns>
        public static T Reader<T>(string file, string section) where T : new()
        {
            try
            {
                var files = IoFiles.GetFileMapPath(file);
                var config = Configuration.LoadFromFile(files);
                var sections = config[section];
                return sections == null ? default(T) : config[section].ToObject<T>();// .CreateObject<T>();
            }
            catch (Exception exception)
            {
                LogHelper.Log(exception);
            }
            return default(T);
        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="file">CFG/INI</param>
        /// <param name="section">模块</param>
        /// <param name="obj">类</param>
        public static void Save(string file, string section,object obj)
        {
            var config = Configuration.LoadFromFile(file);
            //var sections = config[section];
            //config[section] = Section.FromObject(section, obj);
            config.SaveToFile(file);
        }
    }
}
