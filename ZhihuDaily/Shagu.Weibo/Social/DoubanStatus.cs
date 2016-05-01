/* ==============================================================================
 * 功能描述：DoubanStatus  
 * 创 建 者：贤凯
 * 创建日期：2/8/2015 5:25:18 PM
 * ==============================================================================*/

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class DoubanStatus
    {
        [JsonProperty("category")]
        public object Category { get; set; }

        [JsonProperty("entities")]
        public object Entities { get; set; }

        [JsonProperty("reshared_count")]
        public int ResharedCount { get; set; }

        [JsonProperty("layout")]
        public int Layout { get; set; }

        [JsonProperty("from_subscription")]
        public bool FromSubscription { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("can_reply")]
        public int CanReply { get; set; }

        [JsonProperty("liked")]
        public bool Liked { get; set; }

        [JsonProperty("source")]
        public object Source { get; set; }

        [JsonProperty("like_count")]
        public int LikeCount { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("user")]
        public DoubanUser User { get; set; }

        [JsonProperty("is_follow")]
        public bool IsFollow { get; set; }

        [JsonProperty("has_photo")]
        public bool HasPhoto { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attachments")]
        public object[] Attachments { get; set; }
    }
}
