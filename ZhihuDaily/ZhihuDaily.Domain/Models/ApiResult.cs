using System;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class ApiResult
    {
        [JsonProperty("now")]
        public DateTime Now { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }

    }
    public class ApiResult<T>:ApiResult
    {
      
        [JsonProperty("result")]
        public T Result { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
