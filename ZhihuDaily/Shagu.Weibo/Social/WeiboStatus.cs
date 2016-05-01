/* ==============================================================================
 * 功能描述：WeiboStatus  
 * 创 建 者：贤凯
 * 创建日期：2/8/2015 3:27:58 PM
 * ==============================================================================*/

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class WeiboStatus
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("mid")]
        public string Mid { get; set; }

        [JsonProperty("idstr")]
        public string Idstr { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("favorited")]
        public bool Favorited { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        [JsonProperty("in_reply_to_status_id")]
        public string InReplyToStatusId { get; set; }

        [JsonProperty("in_reply_to_user_id")]
        public string InReplyToUserId { get; set; }

        [JsonProperty("in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        [JsonProperty("geo")]
        public object Geo { get; set; }

        [JsonProperty("user")]
        public WeiboUser User { get; set; }

        [JsonProperty("annotations")]
        public object[] Annotations { get; set; }

        [JsonProperty("reposts_count")]
        public int RepostsCount { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("attitudes_count")]
        public int AttitudesCount { get; set; }

        [JsonProperty("mlevel")]
        public int Mlevel { get; set; }

        [JsonProperty("visible")]
        public object Visible { get; set; }
    }
}
