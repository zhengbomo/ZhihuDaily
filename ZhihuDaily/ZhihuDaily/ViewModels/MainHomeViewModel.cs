using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Extends;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;

namespace ZhihuDaily.ViewModels
{
    public class MainHomeViewModel : Screen, IReloadListVM
    {
        private int _carouselSelectedIndex = -1;

        public int CarouselSelectedIndex
        {
            get { return _carouselSelectedIndex; }
            set
            {
                if (_carouselSelectedIndex != value)
                {
                    _carouselSelectedIndex = value;
                    NotifyOfPropertyChange(nameof(CarouselSelectedIndex));
                }
            }
        }

        private string lastData;

        private readonly IFrameManager _frameManager;
        public ObservableCollection<TopStoryInfo> Carousels { get; set; }
        public ObservableCollectionExt<StoryInfo> Articles { get; set; }

        public MainHomeViewModel()
        {
            Carousels = new ObservableCollection<TopStoryInfo>();
            Articles = new ObservableCollectionExt<StoryInfo> { PageCount = 20 };
            if (DesignMode.DesignModeEnabled)
            {
                var result = DesignDataService.GetRecommendStory();
                foreach (var carouse in result.TopStories)
                {
                    Carousels.Add(carouse);
                }

                foreach (var storyInfo in result.Stories)
                {
                    Articles.Add(storyInfo);
                }
                lastData = result.Date;
            }
        }

        public MainHomeViewModel(IFrameManager frameManager) : this()
        {
            _frameManager = frameManager;
            RequestCache();
        }

        public async void RequestCache()
        {
            if (!Articles.IsLoaded)
            {
                Articles.IsLoaded = true;

                var result = await CacheManager.GetRecommendStory();
                if (result != null)
                {
                    Carousels.Clear();
                    foreach (var carouse in result.TopStories)
                    {
                        Carousels.Add(carouse);
                    }
                    CarouselSelectedIndex = -1;
                    CarouselSelectedIndex = 0;

                    Articles.Clear();
                    foreach (var storyInfo in result.Stories)
                    {
                        Articles.Add(storyInfo);
                    }
                    Articles.HasMore = false;
                }

                RequestItems();
            }
        }

        public void Reload()
        {
            RequestItems();
        }

        public async void RequestItems()
        {
            if (!Articles.IsRefreshing)
            {
                Articles.IsRefreshing = true;

                var result = await DataService.GetRecommendStory();
                RequestUtil.DoResult(result, async res =>
                {
                    await CacheManager.SetRecommendStory(res);
                    //缓存
                    Carousels.Clear();
                    foreach (var carouse in res.TopStories)
                    {
                        Carousels.Add(carouse);
                    }
                    CarouselSelectedIndex = -1;
                    CarouselSelectedIndex = 0;

                    Articles.Clear();
                    foreach (var storyInfo in res.Stories)
                    {
                        Articles.Add(storyInfo);
                    }
                    Articles.HasMore = true;

                    lastData = res.Date;
                });

                Articles.IsRefreshing = false;
            }
        }

        public async void LoadItems()
        {
            Articles.IsLoading = true;

            var result = await DataService.GetHomeStory(lastData);
            RequestUtil.DoResult(result, res =>
            {
                foreach (var storyInfo in res.Stories)
                {
                    Articles.Add(storyInfo);
                }

                lastData = res.Date;
            });

            Articles.IsLoading = false;
        }


        #region Event



        public void CarouselClick(TopStoryInfo carousel)
        {
            switch (carousel.Type)
            {
                case 0:
                case 2:
                    _frameManager.RightFrameClearAndNav((navigationService) =>
                    {
                        navigationService.For<ArticleDetailViewModel>()
                            .WithParam(vm => vm.Image, carousel.Image)
                            .WithParam(vm => vm.ArticleId, carousel.Id)
                            .WithParam(vm => vm.Multipic, false)
                            .Navigate();
                    });
                    break;
                case 1:
                    _frameManager.RightFrameClearAndNav((navigationService) =>
                    {
                        navigationService.For<WebViewArticleViewModel>()
                            .WithParam(vm => vm.Image, carousel.Image)
                            .WithParam(vm => vm.ArticleId, carousel.Id)
                            .WithParam(vm => vm.Multipic, false)
                            .Navigate();
                    });
                    break;

            }
        }

        #endregion

     
    }
}