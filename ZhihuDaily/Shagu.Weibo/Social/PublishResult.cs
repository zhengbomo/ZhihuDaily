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
// 10/16/2014 11:44:43 AM		bomo 		Initial creation 
//
// *************************************************

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class PublishResult
    {
        [JsonProperty("error")]
        public ErrorResult Error { get; set; }
    }
}
