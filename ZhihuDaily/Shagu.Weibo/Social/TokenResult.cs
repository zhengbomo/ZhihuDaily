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
// 10/04/2014 15:25:06		bomo 		Initial creation 
//
// *************************************************

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class TokenResult
    {
        #region Common

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        
        #endregion

        #region Weibo Token

        [JsonProperty("remind_in")]
        public string RemindIn { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }
        
        #endregion

        #region Douban

        [JsonProperty("douban_user_name")]
        public string DoubanUserName { get; set; }

        [JsonProperty("douban_user_id")]
        public string DoubanUserId { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        
        #endregion
    }
}
