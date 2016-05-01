using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class FavoriteResult
    {

        [JsonProperty("stories")]
        public List<StoryInfo> Stories { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("last_time")]
        public long LastTime { get; set; }
    }
}
