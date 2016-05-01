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
// 10/04/2014 15:23:59		bomo 		Initial creation 
//
// *************************************************

using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Shagu.Weibo.Social
{
    public class WeiboClient : SocialClientBase
    {
        public WeiboClient(string appId, string appSecret, string appRedirectUrl)
        {
            AppId = appId;
            AppSecret = appSecret;
            AppRedirectUrl = appRedirectUrl;
            RootUrl = "https://api.weibo.com/";
            AuthorizeUrl = string.Format(
                "https://api.weibo.com/oauth2/authorize?client_id={0}&response_type=code&redirect_uri={1}&display=mobile",
                AppId, AppRedirectUrl);

            TokenUrl = "oauth2/access_token";
            PublishUrl = "https://api.weibo.com/2/statuses/update.json";
            PublishWithImageUrl = "https://upload.api.weibo.com/2/statuses/upload.json";
        }

        public override List<KeyValuePair<string, string>> GetPublishParams(string token, string conent)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("source", AppId),
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("status", conent),
                new KeyValuePair<string, string>("visible", "0"),
            };
        }

        public override MultipartFormDataContent GetPublishWithImageParams(string token, string content, Stream stream)
        {
            return new MultipartFormDataContent
            {
                {new StringContent(token), "access_token"},
                {new StringContent(content), "status"},
                {new StringContent("0"), "visible"},
                CreateImageContent(stream, "pic")
            };
        }
    }
}
