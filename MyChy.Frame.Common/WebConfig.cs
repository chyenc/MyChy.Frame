
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common
{
    public static class WebConfig
    {
        /// <summary>
        /// 根据配置节key获取连接字符串
        /// </summary>
        public static T AppSettingsName<T>(string strSettingName, T defaultValue)
        {
            return ConfigurationManager.AppSettings[strSettingName].To<T>(defaultValue);
        }

        /// <summary>
        /// 根据配置节key获取连接字符串
        /// </summary>
        public static T AppSettingsName<T>(string strSettingName)
        {
            return ConfigurationManager.AppSettings[strSettingName].To<T>();
        }

        public static string CdnServer = AppSettingsName<string>("CdnServer");

        public static string ImageServer = AppSettingsName<string>("ImageServer");

        public static string TemplatePath = AppSettingsName<string>("TemplatePath");

        public static readonly int WinXinId = AppSettingsName<int>("WeiXinId", 1);

        //public static string DefualtWebSite = WebConfig.AppSettingsName<string>("DefualtWebSite");

        public static readonly string WeiXinDesKey = WebConfig.AppSettingsName<string>("WeiXinDesKey", "46$6d3fg");


    }
}
