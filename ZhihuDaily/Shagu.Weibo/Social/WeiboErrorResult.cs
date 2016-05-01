/* ==============================================================================
 * 功能描述：WeiboErrorResult  
 * 创 建 者：贤凯
 * 创建日期：2/10/2015 11:12:37 PM
 * ==============================================================================*/

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class WeiboErrorResult
    {

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }
    }
}
