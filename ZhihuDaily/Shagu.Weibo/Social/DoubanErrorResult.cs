/* ==============================================================================
 * 功能描述：DoubanErrorResult  
 * 创 建 者：贤凯
 * 创建日期：2/10/2015 11:25:16 PM
 * ==============================================================================*/

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class DoubanErrorResult
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
