using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Extends;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;

namespace ZhihuDaily.ViewModels
{
    public class ArticleCollectionViewModel : Screen
    {
        private readonly CollectionService _collectionService;
        private readonly IFrameManager _frameManager;
        public ObservableCollectionExt<CollectionInfo> Articles { get; set; }

        public ArticleCollectionViewModel(CollectionService collectionService, IFrameManager frameManager)
        {
            _collectionService = collectionService;
            _frameManager = frameManager;

            Articles = new ObservableCollectionExt<CollectionInfo> {PageCount = 20};

            RequestItems();
        }

        private void RequestItems()
        {
            Articles.Page = 0;
            var articleInfos = _collectionService.GetArticleList(Articles.Page + 1, Articles.PageCount);
            Articles.Clear();
            foreach (var collectionInfo in articleInfos)
            {
                Articles.Add(collectionInfo);
            }

            Articles.HasMore = articleInfos.Count == Articles.PageCount;
            Articles.Page++;
        }

        public void LoadItems()
        {
            var articleInfos = _collectionService.GetArticleList(Articles.Page + 1, Articles.PageCount);
            foreach (var collectionInfo in articleInfos)
            {
                Articles.Add(collectionInfo);
            }
            Articles.HasMore = articleInfos.Count == Articles.PageCount;
            Articles.Page++;
        }
    }
}
