/* ==============================================================================
 * 功能描述：RecommendAppInfo  
 * 创 建 者：贤凯
 * 创建日期：2/9/2015 11:24:29 AM
 * ==============================================================================*/

using Shagu.Utils;

namespace Shagu.UI.Models
{
    public enum SupportType
    {
        Mobile, Desktop, Both
    }

    public class RecommendAppInfo
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ProductId { get; set; }
        public string Descriptio { get; set; }
        public SupportType SupportType { get; set; }
    }
}
