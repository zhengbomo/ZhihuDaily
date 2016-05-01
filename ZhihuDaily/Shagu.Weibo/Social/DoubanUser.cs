/* ==============================================================================
 * 功能描述：DoubanUser  
 * 创 建 者：贤凯
 * 创建日期：2/8/2015 5:23:13 PM
 * ==============================================================================*/

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class DoubanUser
    {
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("small_avatar")]
        public string SmallAvatar { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("large_avatar")]
        public string LargeAvatar { get; set; }
    }
}
