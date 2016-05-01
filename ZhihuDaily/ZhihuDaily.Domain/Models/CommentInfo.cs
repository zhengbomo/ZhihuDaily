using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class CommentInfo
    {
        [JsonProperty("own")]
        public bool Own { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("time")]
        public double Time { get; set; }

        [JsonProperty("voted")]
        public bool Voted { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("reply_to")]
        public CommentInfo ReplyTo { get; set; }


        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
