using System.Linq;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Extends;
using Shagu.Utils.Utils;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Models;
using ZhihuDaily.Util;

namespace ZhihuDaily.ViewModels
{
    public class CommentListViewModel : Screen
    {
        private readonly IFrameManager _frameManager;
        private readonly GlobalInfoManager _globalInfoManager;

        private bool _isLongComment = true;
        private int _beforeId;
        private int _articleId;

        public int ArticleId
        {
            get { return _articleId; }
            set
            {
                _articleId = value;
                RequsetItems();
            }
        }


        public ObservableCollectionExt<ArticleParagraph> Paragraphs { get; set; }

        public CommentListViewModel()
        {
            Paragraphs = new ObservableCollectionExt<ArticleParagraph>
            {
//                HasMore = true
            };
        }

        public CommentListViewModel(GlobalInfoManager globalInfoManager, IFrameManager frameManager):this()
        {
            _globalInfoManager = globalInfoManager;
            _frameManager = frameManager;
        }

        public void RequsetItems()
        {
            _isLongComment = true;
            _beforeId = 0;
            Paragraphs.Clear();
            LoadItems();
        }


        public async void LoadItems()
        {
            Paragraphs.IsLoading = true;

            var result = await DataService.GetStoryComment(_globalInfoManager.AccessToken, ArticleId, _isLongComment, _beforeId);
            RequestUtil.DoResult(result, res =>
            {
                if (_isLongComment)
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

                    if (!_isLongComment)
                    {
                        _beforeId = res.Comments.Last().Id;
                    }
                    Paragraphs.HasMore = true;
                }
                else if (_isLongComment)
                {
                    //没有长评论
                    Paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeNoLongComment,
                    });

                }
                else
                {
                    Paragraphs.HasMore = false;
                }

                if (_isLongComment)
                {
                    Paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeCommentTitle,
                        Content = "短评论"
                    });
                    _isLongComment = false;

                    LoadItems();
                }


            });

            Paragraphs.IsLoading = false;
        }


        public void Nav2CommmentView()
        {
            if (_globalInfoManager.IsLogin)
            {
                _frameManager.RightFrameAndNav(navigationService =>
                {
                    navigationService.For<PublichCommentViewModel>()
                        .WithParam(vm => vm.ArticleId, ArticleId)
                        .Navigate();
                });
            }
            else
            {
                ToastHelper.ToastError("你还没有登陆哦", _globalInfoManager.IsNightMode);
            }
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
