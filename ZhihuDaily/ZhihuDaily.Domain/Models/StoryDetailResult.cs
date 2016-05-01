using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class StoryDetailResult
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("recommenders")]
        public List<UserInfo> Recommenders { get; set; }

        [JsonProperty("share_url")]
        public string ShareUrl { get; set; }

        [JsonProperty("js")]
        public object[] Js { get; set; }

        [JsonProperty("theme")]
        public CategoryInfo Theme { get; set; }

        [JsonProperty("ga_prefix")]
        public string GaPrefix { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("css")]
        public string[] Css { get; set; }

        [JsonProperty("image_source")]
        public string ImageSource { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("section")]
        public CategoryInfo Section { get; set; }
    }

}
