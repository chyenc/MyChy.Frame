﻿
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

    }
}
