using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class CategoryInfo
    {
        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
