using System.Linq;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Extends;
using Shagu.Utils.Utils;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Models;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;
using ZhihuDaily.Views;

namespace ZhihuDaily.ViewModels
{
    public class ArticleDetailViewModel : Screen
    {
        private int _articleId;

        public int ArticleId
        {
            get { return _articleId; }
            set
            {
                if (_articleId != value)
                {
                    _articleId = value;
                    RequestArticle();
                }
            }
        }


        public bool Multipic { get; set; }
        public string Image { get; set; }
        
        private readonly IFrameManager _frameManager;
        private readonly CollectionService _collectionService;
        private readonly GlobalInfoManager _globalInfoManager;
        private readonly IEventAggregator _eventAggregator;
        private bool isLongComment = true;
        private int beforeId = 0;

        public StoryDetailResult ArticleInfo { get; set; }

        protected string _articleHtml;

        public ObservableCollectionExt<ArticleParagraph> Paragraphs { get; set; }

        public ArticleDetailViewModel()
        {
            Paragraphs = new ObservableCollectionExt<ArticleParagraph>
            {
                HasMore = true
            };
        }

        public ArticleDetailViewModel(GlobalInfoManager globalInfoManager,
            IFrameManager frameManager, CollectionService collectionService, IEventAggregator eventAggregator):this()
        {
            _frameManager = frameManager;
            _collectionService = collectionService;
            _globalInfoManager = globalInfoManager;
            _eventAggregator = eventAggregator;
        }

        public async void RequestArticle()
        {
            LoadCount();

            Paragraphs.IsRefreshing = true;

            var result = await DataService.GetStoryDetail(_globalInfoManager.AccessToken, ArticleId);

            await RequestUtil.DoResult(result, async res =>
            {
                //缓存
                await CacheManager.SetStoryDetail(ArticleId, res);

                LoadArticle(res);
            }, error =>
            {
                //失败，则请求缓存
                RequestCache();
            });

            Paragraphs.IsRefreshing = false;
        }

        private void LoadArticle(StoryDetailResult storyDetail)
        {
            ArticleInfo = storyDetail;
            NotifyOfPropertyChange(nameof(ArticleInfo));

            _articleHtml = storyDetail.Body;
            var paragraphs = ArticleHelper.AnalyzeHtml(_articleHtml);
            Paragraphs.Clear();
            foreach (var articleParagraph in paragraphs)
            {
                Paragraphs.Add(articleParagraph);
            }

            Paragraphs.Add(new ArticleParagraph {Type = ArticleParagraphType.ArticleParagraphTypeAd});

            //加载评论
//            LoadItems();
        }

        public async void LoadItems()
        {
            Paragraphs.IsLoading = true;

            var result = await DataService.GetStoryComment(_globalInfoManager.AccessToken, ArticleId, isLongComment, beforeId);
            RequestUtil.DoResult(result, res =>
            {
                if (isLongComment)
                {
                    Paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeCommentTitle,
                        Content = "长评论"
                    });
                }
                if (res.Comments?.Count > 0)
                {
                    foreach (var comment in res.Comments)
                    {
                        Paragraphs.Add(new ArticleParagraph
                        {
                            Type = ArticleParagraphType.ArticleParagraphTypeComment,
                            TagValue = comment
                        });
                    }

                    if (!isLongComment)
                    {
                        beforeId = res.Comments.Last().Id;
                    }
                }
                else if(isLongComment)
                {
                    //没有长评论
                    Paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeNoLongComment,
                    });
                    
                }

                if (isLongComment)
                {
                    Paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeCommentTitle,
                        Content = "短评论"
                    });
                    isLongComment = false;

                    LoadItems();
                }
            });

            Paragraphs.IsLoading = false;
        }




        private async void RequestCache()
        {
            //读取缓存
            var result = await CacheManager.GetStoryDetail(ArticleInfo.Id);
            if (result != null)
            {
                LoadArticle(result);
            }
        }

        public async void Collect()
        {
            if (_globalInfoManager.IsLogin)
            {
                if (CounterList != null)
                {
                    //请求成功
                    var result =
                        await DataService.SetFavorite(_globalInfoManager.AccessToken, ArticleId, !CounterList.Favorite);
                    RequestUtil.DoResult(result, operation =>
                    {
                        CounterList.Favorite = !CounterList.Favorite;
                        ToastHelper.ToastInfo(CounterList.Favorite ? "收藏成功" : "取消收藏成功", _globalInfoManager.IsNightMode);
                    });
                }
            }
            else
            {
                ToastHelper.ToastError("你还没有登陆哦 ^o^", _globalInfoManager.IsNightMode);
            }
        }

        public async void Like()
        {
            if (_globalInfoManager.IsLogin)
            {
                if (CounterList != null)
                {
                    //请求成功
                    var result =
                        await DataService.SetSupport(_globalInfoManager.AccessToken, ArticleId, !CounterList.Favorite);
                    RequestUtil.DoResult(result, operation =>
                    {
                        CounterList.IsVoted = !CounterList.IsVoted;
                        ToastHelper.ToastInfo(CounterList.IsVoted ? "赞成功" : "取消赞成功", _globalInfoManager.IsNightMode);
                    });
                }
            }
            else
            {
                ToastHelper.ToastError("你还没有登陆哦 ^o^", _globalInfoManager.IsNightMode);
            }
        }




        #region LoadCount

        public CounterList CounterList { get; set; }

        public bool CountLoaded => CounterList != null;

        public async void LoadCount()
        {
            var countResult = await DataService.GetCounterList(_globalInfoManager.AccessToken, ArticleId);
            RequestUtil.DoResult(countResult, (count) =>
            {
                CounterList = count;
                NotifyOfPropertyChange(nameof(CounterList));
                NotifyOfPropertyChange(nameof(CountLoaded));
            });
        }

        #endregion

        public void Nav2CommmentView()
        {
            _frameManager.RightFrameAndNav(navigationService =>
            {
                navigationService.For<CommentListViewModel>()
                    .WithParam(vm => vm.ArticleId, ArticleId)
                    .Navigate();
            });
        }

        #region Comment

        public void ReplyComment(ArticleParagraph articleParagraph)
        {
            if (_globalInfoManager.IsLogin)
            {
                var commentInfo = (CommentInfo)articleParagraph.TagValue;

                _frameManager.RightFrameAndNav(navigationService =>
                {
                    navigationService.For<PublichCommentViewModel>()
                        .WithParam(vm => vm.ArticleId, ArticleId)
                        .WithParam(vm => vm.ReplyId, commentInfo.Id)
                        .WithParam(vm => vm.ReplyName, commentInfo.Author)
                        .Navigate();
                });
            }
            else
            {
                ToastHelper.ToastError("你还没有登陆哦 ^o^", _globalInfoManager.IsNightMode);
            }
        }
        public async void DeleteComment(ArticleParagraph articleParagraph)
        {
            if (_globalInfoManager.IsLogin)
            {
                if (ContentDialogResult.Primary == await MessageBox.ShowAsync("确定要删除吗 :(", "提示"))
                {
                    var commentInfo = (CommentInfo)articleParagraph.TagValue;
                    //请求成功
                    var result =
                        await DataService.DelComment(_globalInfoManager.AccessToken, commentInfo.Id);
                    RequestUtil.DoResult(result, operation =>
                    {
                        Paragraphs.Remove(articleParagraph);
                        ToastHelper.ToastInfo("删除成功", _globalInfoManager.IsNightMode);
                    });
                }
            }
            else
            {
                ToastHelper.ToastError("你还没有登陆哦 ^o^", _globalInfoManager.IsNightMode);
            }
        }

        public async void ReportComment(ArticleParagraph articleParagraph)
        {
            if (ContentDialogResult.Primary == await MessageBox.ShowAsync("太好了，我们一起净化评论区吧 : )", "提示", "确认举报"))
            {
                var commentInfo = (CommentInfo)articleParagraph.TagValue;
                //请求成功
                var result =
                    await DataService.ReportComment(_globalInfoManager.AccessToken, commentInfo.Id);
                RequestUtil.DoResult(result, operation =>
                {
                    ToastHelper.ToastInfo("举报成功", _globalInfoManager.IsNightMode);
                });
            }
        }


        #endregion
    }
}
