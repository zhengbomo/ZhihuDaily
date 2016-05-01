using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class CommentResult
    {
        [JsonProperty("comments")]
        public List<CommentInfo> Comments { get; set; }
    }
}
