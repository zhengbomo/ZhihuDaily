using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class RecommendStoryResult
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("stories")]
        public List<StoryInfo> Stories { get; set; }

        [JsonProperty("top_stories")]
        public List<TopStoryInfo> TopStories { get; set; }

    }
}
