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
// 10/04/2014 15:24:20		bomo 		Initial creation 
//
// *************************************************

using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Shagu.Weibo.Social
{
    public class RenrenClient : SocialClientBase
    {
        public RenrenClient(string appId, string appSecret, string appRedirectUrl)
        {
            AppId = appId;
            AppSecret = appSecret;
            AppRedirectUrl = appRedirectUrl;
            RootUrl = "https://graph.renren.com/";
            AuthorizeUrl = string.Format(
                    "https://graph.renren.com/oauth/authorize?client_id={0}&redirect_uri={1}&response_type=code&display=mobile&scope=status_update+photo_upload+read_user_photo+publish_share",
                    AppId, AppRedirectUrl);

            TokenUrl = "oauth/token";
            PublishUrl = "https://api.renren.com/v2/status/put";

            PublishWithImageUrl = "https://api.renren.com/v2/photo/upload";
        }

        public override List<KeyValuePair<string, string>> GetPublishParams(string token, string content)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("content", content),
            };
        }

        public override MultipartFormDataContent GetPublishWithImageParams(string token, string content, Stream stream)
        {
            return new MultipartFormDataContent
            {
                {new StringContent(token), "access_token"},
                {new StringContent(content), "description"},
                CreateImageContent(stream, "file")
            };
        }
    }
}
