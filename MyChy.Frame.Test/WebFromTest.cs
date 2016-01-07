using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Common.Helper;
using NUnit.Framework;

namespace MyChy.Frame.Test
{
    [TestFixture]
    public class WebFromTest
    {
        [Test]
        public void Test()
        {

            var entity = EncryptServerHelper.ShowEncrypt("o8KB3uBl_0QJFEpnFUCnyYMPpZ9Y", "9EAFA395-3AFC-4D41-93CD-F801FBF8C8A3");
            var dictionary = new Dictionary<object, object>
                        {
                            {"Encrypt",entity.Encrypt},
                            {"Sign",entity.Sign},
                        };
            var postDataStr = dictionary.ToQueryString();
            var requset = WebHelper.WebFormPost("http://hyinvite.21move.net/api/BindingUnbundling/", postDataStr);
        }
    }
}
