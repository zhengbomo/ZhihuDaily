using Caliburn.Micro;
using Shagu.UI.Utils;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;

namespace ZhihuDaily.ViewModels
{
    public class WebViewArticleViewModel : ArticleDetailViewModel
    {

        public WebViewArticleViewModel(GlobalInfoManager globalInfoManager,
            IFrameManager frameManager, CollectionService collectionService, IEventAggregator eventAggregator)
            : base(globalInfoManager, frameManager, collectionService, eventAggregator)
        {
            _articleHtml = string.Empty;
        }
    }
}
