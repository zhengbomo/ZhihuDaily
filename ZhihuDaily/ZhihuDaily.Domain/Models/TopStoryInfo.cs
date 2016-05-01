using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class TopStoryInfo
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ga_prefix")]
        public string GaPrefix { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

    }
}
