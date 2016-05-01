// *************************************************
// 
// 作者：bomo
// 小组：WP开发组
// 创建日期：2014/5/25 0:49:04
// 版本号：V1.00
// 说明:
// 
// *************************************************
// 
// 修改历史: 
// Date				WhoChanges		Made 
// 10/16/2014 11:43:02 AM		bomo 		Initial creation 
//
// *************************************************

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class ErrorResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
