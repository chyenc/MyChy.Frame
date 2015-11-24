
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
        public static readonly string WinXinToken = AppSettingsName<string>("WinXinToken", "BFAE5520E05E43AAB4F2876FDA9ED867");


        public static readonly int WeiXinTokenId = AppSettingsName<int>("WeiXinTokenId", 1);
        public static readonly string WeiXinTokenAppId = AppSettingsName<string>("WeiXinTokenAppId", "wx7a04ca1df8292ab8");
        
        
        public static readonly string WinXinAppId = AppSettingsName<string>("WeiXinAppId", "wx7a04ca1df8292ab8");

        public static readonly int WinXinId = AppSettingsName<int>("WeiXinId", 1);

        public static readonly string WebDefUrl = AppSettingsName<string>("WebDefUrl", "");

        public static readonly bool WebJsSkdDebug = AppSettingsName<bool>("WebJsSkdDebug", false);

        public static readonly string CdnServer = AppSettingsName<string>("CdnServer", "");

        public static readonly string OpenIdDesKey = AppSettingsName<string>("OpenIdDesKey", "do&4f@5l");


        public static readonly string WinXinEncodingAesKey = AppSettingsName<string>("WinXinEncodingAesKey", "CmUodUez1VtwGDmYOpc8zOOsWPhxZB9ONtScIFTrAtY");

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

        public static string WinXinNewWzj = "您输入的兑换码有误,请再试一次";

        public static string WinXinNewWzjError = "您输入的兑换码有误,请购买汇源促销装后再参加活动";

        public static string WinXinNewYdj = "已兑奖";

        public static int WinXinCodeMinQ = 10;

        public static int WinXinCodeMaxQ = 39;

        public static string DESKey = "124xu@1a";

        public static string WinXinH5Url = "hyweb.sh.1251228687.clb.myqcloud.com/?code={0}";



        public static int WinXinGiftCodeId = 3;

        public static string WinXinH5ErrorUrl = "http://mp.weixin.qq.com/s?__biz=MzA5NzUyODIxNQ==&mid=201754287&idx=1&sn=617ec64cd09bd797d3ac182c5cba0106#rd";

        public static int WinXinLockOpenidMax = 5;


        public static bool WinXinIsLog = true;

        public static string DefualtWebSite = WebConfig.AppSettingsName<string>("DefualtWebSite");
        //public static string WinXinToken = "";
        //public static string WinXinToken = "";

    }
}
