using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class CategoryResult
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("subscribed")]
        public object[] Subscribed { get; set; }

        [JsonProperty("others")]
        public List<CategoryInfo> Categorys { get; set; }
    }
}
