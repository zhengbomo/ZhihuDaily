/* ==============================================================================
 * 功能描述：RecommendAppViewModel  
 * 创 建 者：贤凯
 * 创建日期：2/9/2015 10:45:06 AM
 * ==============================================================================*/

using ZhihuDaily.Domain.Core;
using Shagu.UI.ViewModels;

namespace ZhihuDaily.ViewModels
{
    public class RecommendAppViewModel : RecommendAppViewModelBase
    {
        public RecommendAppViewModel() : base(Constants.ProductId)
        {
        }
    }
}
