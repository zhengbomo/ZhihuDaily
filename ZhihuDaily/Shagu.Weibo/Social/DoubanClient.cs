// *************************************************
// 
// 作者：bomo
// 小组：WP开发组
// 创建日期：2014/5/25 0:49:04
// 版本号：V1.00
// 说明:
// 
// *************************************************
// 
// 修改历史: 
// Date				WhoChanges		Made 
// 10/04/2014 15:24:41		bomo 		Initial creation 
//
// *************************************************

using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Shagu.Weibo.Social
{
    public class DoubanClient : SocialClientBase
    {
        

        public DoubanClient(string appId, string appSecret, string appRedirectUrl)
        {
            AppId = appId;
            AppSecret = appSecret;
            AppRedirectUrl = appRedirectUrl;

            RootUrl = "https://www.douban.com/";
            AuthorizeUrl = string.Format(
                "{0}service/auth2/auth?client_id={1}&redirect_uri={2}&response_type=code&scope=shuo_basic_r,shuo_basic_w,douban_basic_common",
                RootUrl, AppId, AppRedirectUrl);


            TokenUrl = "service/auth2/token";
            PublishUrl = "https://api.douban.com/shuo/v2/statuses/";
            PublishWithImageUrl = PublishUrl;

        }

        public override List<KeyValuePair<string, string>> GetPublishParams(string token, string content)
        {
            throw new System.NotImplementedException();
        }

        public override MultipartFormDataContent GetPublishWithImageParams(string token, string content, Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}
