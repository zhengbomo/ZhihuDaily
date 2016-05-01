using System.Linq;
using Caliburn.Micro;
using Shagu.Utils.Extends;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;

namespace ZhihuDaily.ViewModels
{
    public class CategoryDetailViewModel : Screen, IReloadListVM
    {

        public string Title { get; set; }
        private int _categoryId;

        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    NotifyOfPropertyChange(nameof(Title));
                    LoadCache();
                }
            }
        }

        public ObservableCollectionExt<StoryInfo> Stories { get; set; }
        public CategoryDetailResult CategoryDetail { get; set; }


        public CategoryDetailViewModel()
        {
            Stories = new ObservableCollectionExt<StoryInfo> {PageCount = 20};
        }

        private async void LoadCache()
        {
            Stories.Clear();
            CategoryDetail = null;
            NotifyOfPropertyChange(nameof(CategoryDetail));

            var result = await CacheManager.GetCategoryDetail(CategoryId);
            if (result != null)
            {
                CategoryDetail = result;
                NotifyOfPropertyChange(nameof(CategoryDetail));
                foreach (var storyInfo in result.Stories)
                {
                    Stories.Add(storyInfo);
                }

                
            }

            RequestItems();
        }

        public void Reload()
        {
            RequestItems();
        }

        public async void RequestItems()
        {
            if (!Stories.IsRefreshing)
            {
                Stories.IsRefreshing = true;

                var result = await DataService.GetCategoryDetail(CategoryId, 0);
                RequestUtil.DoResult(result, async res =>
                {
                    await CacheManager.SetCategoryDetail(CategoryId, res);

                    CategoryDetail = res;
                    NotifyOfPropertyChange(nameof(CategoryDetail));

                    //缓存
                    Stories.Clear();
                    foreach (var storyInfo in res.Stories)
                    {
                        Stories.Add(storyInfo);
                    }

                    Stories.HasMore = true;
                });

                Stories.IsRefreshing = false;
            }
        }

        public async void LoadItems()
        {
            Stories.IsLoading = true;

            var lastStory = Stories.LastOrDefault();
            if (lastStory != null)
            {
                var result = await DataService.GetCategoryDetail(CategoryId, Stories.Last().Id);
                RequestUtil.DoResult(result, res =>
                {
                    foreach (var storyInfo in res.Stories)
                    {
                        Stories.Add(storyInfo);
                    }

                    Stories.HasMore = true;
                });
            }
            Stories.IsLoading = false;
        }
    }

}
