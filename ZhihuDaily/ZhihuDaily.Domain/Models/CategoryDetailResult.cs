using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class CategoryDetailResult
    {
        [JsonProperty("stories")]
        public List<StoryInfo> Stories { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }

        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("editors")]
        public List<UserInfo> Editors { get; set; }

        [JsonProperty("image_source")]
        public string ImageSource { get; set; }
    }
}
