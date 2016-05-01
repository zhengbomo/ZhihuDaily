using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class StoryListResult
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("stories")]
        public List<StoryInfo> Stories { get; set; }

    }
}
