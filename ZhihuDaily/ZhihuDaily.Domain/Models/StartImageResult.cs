using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class StartImageResult
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("img")]
        public string Img { get; set; }
    }
}
