using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Shagu.Weibo.Social;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;

namespace ZhihuDaily.Utils
{
    public class AuthHelper
    {
        public static async Task<RequestResult<TokenResult>> GetWeiboToken(WeiboClient client, string code, CancellationToken token = new CancellationToken())
        {

            var param = new Dictionary<string, string>
            {
                {"client_id", client.AppId},
                {"client_secret", client.AppSecret},
                {"grant_type", "authorization_code"},
                {"redirect_uri", client.AppRedirectUrl},
                {"code", code}
            };

            var url = client.RootUrl + client.TokenUrl;
            return await DataService.Post<TokenResult>(url, new FormUrlEncodedContent(param), null, token);
        }

    }
}
