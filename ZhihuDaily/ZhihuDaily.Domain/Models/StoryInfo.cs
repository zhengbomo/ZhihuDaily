using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace ZhihuDaily.Domain.Models
{
    public class StoryInfo
    {
        [Column("id"), PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Column("type")]
        [JsonProperty("type")]
        public int Type { get; set; }

        [Column("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Ignore]
        [JsonProperty("images")]
        public List<string> Images { get; set; }

        [Column("multipic")]
        [JsonProperty("multipic")]
        public bool? Multipic { get; set; }

        [Column("ga_prefix")]
        [JsonProperty("ga_prefix")]
        public string GaPrefix { get; set; }
    }
}
