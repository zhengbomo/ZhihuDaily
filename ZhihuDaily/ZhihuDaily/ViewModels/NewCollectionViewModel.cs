using System.Linq;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Extends;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;

namespace ZhihuDaily.ViewModels
{
    public class NewCollectionViewModel : Screen
    {
        private readonly IFrameManager _frameManager;
        private readonly GlobalInfoManager _globalInfoManager;
        public ObservableCollectionExt<StoryInfo> Articles { get; set; }

        public string CountMessage { get; set; }

        private long _lastTime;

        public NewCollectionViewModel()
        {
            Articles = new ObservableCollectionExt<StoryInfo>();
        }

        public NewCollectionViewModel(IFrameManager frameManager, GlobalInfoManager globalInfoManager):this()
        {
            _frameManager = frameManager;
            _globalInfoManager = globalInfoManager;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            RequestItems();
        }

        public async void RequestItems()
        {
            Articles.IsRefreshing = true;
            Articles.Page = 0;
            var articleResult = await DataService.GetFavorite(_globalInfoManager.AccessToken, 0);

            await RequestUtil.DoResult(articleResult, async res =>
            {
                await CacheManager.SetFaviroteResult(res);
                //缓存
                Articles.Clear();
                if (res.Stories != null)
                {
                    foreach (var collectionInfo in res.Stories)
                    {
                        Articles.Add(collectionInfo);
                    }
                }


                CountMessage = string.Format("（{0}条）", res.Count);
                NotifyOfPropertyChange(nameof(CountMessage));

                _lastTime = res.LastTime;

                Articles.HasMore = Articles.Count < res.Count;
            }, async (error) =>
            {
                //读取缓存
                var result = await CacheManager.GetFaviroteResult();
                if (result?.Stories != null)
                {
                    foreach (var storyInfo in result.Stories)
                    {
                        Articles.Add(storyInfo);
                    }
                }

                Articles.HasMore = false;
            });


            Articles.IsRefreshing = false;
        }

        public async void LoadItems()
        {
            Articles.IsLoading = true;

            var articleResult = await DataService.GetFavorite(_globalInfoManager.AccessToken, _lastTime);

            RequestUtil.DoResult(articleResult, res =>
            {
                if (res != null)
                {
                    foreach (var collectionInfo in res.Stories)
                    {
                        Articles.Add(collectionInfo);
                    }
                }

                _lastTime = res.LastTime;
                Articles.HasMore = res.Count > 0 && Articles.Count < res.Count;
            });

            Articles.IsLoading = false;
        }

        public void Nav2OldFaviroteView()
        {
            _frameManager.CenterNavService.For<ArticleCollectionViewModel>().Navigate();
        }
    }
}
