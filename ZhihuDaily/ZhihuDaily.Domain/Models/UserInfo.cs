using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class UserInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
