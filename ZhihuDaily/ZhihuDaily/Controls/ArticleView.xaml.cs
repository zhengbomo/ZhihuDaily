using System;
using System.Collections.ObjectModel;
using System.Text;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Shagu.Utils;
using Shagu.Utils.Utils;
using ZhihuDaily.Models;
using ZhihuDaily.Util;
using ZhihuDaily.Utils;

namespace ZhihuDaily.Controls
{
    public sealed partial class ArticleView 
    {
        public event EventHandler<ArticleParagraph> ReplyComment;
        public event EventHandler<ArticleParagraph> ReportComment;
        public event EventHandler<ArticleParagraph> DeleteComment;

        public ArticleView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ParagraphsProperty = DependencyProperty.Register(
            "Paragraphs", typeof (ObservableCollection<Paragraph>), typeof (ArticleView), new PropertyMetadata(default(ObservableCollection<ArticleParagraph>)));

        public ObservableCollection<ArticleParagraph> Paragraphs
        {
            get { return (ObservableCollection<ArticleParagraph>) GetValue(ParagraphsProperty); }
            set { SetValue(ParagraphsProperty, value); }
        }
        #region Footer, Header

        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
            "FooterTemplate", typeof(DataTemplate), typeof(ArticleView), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            "Footer", typeof(object), typeof(ArticleView), new PropertyMetadata(default(object)));

        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }


        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof (object), typeof (ArticleView), new PropertyMetadata(default(object)));

        public object Header
        {
            get { return (object) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", typeof (DataTemplate), typeof (ArticleView), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate) GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty WebUrlProperty = DependencyProperty.Register(
            "WebUrl", typeof (string), typeof (ArticleView), new PropertyMetadata(default(string), WebUrlChangedCallback));

        public string WebUrl
        {
            get { return (string)GetValue(WebUrlProperty); }
            set { SetValue(WebUrlProperty, value); }
        }

        private static async void WebUrlChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var articleView = (ArticleView) dependencyObject;

            if (articleView.Paragraphs == null)
            {
                articleView.Paragraphs = new ObservableCollection<ArticleParagraph>();
            }

            if (!string.IsNullOrEmpty(articleView.WebUrl))
            {
                var fileName = string.Format("cacheData\\article\\{0}", Md5Helper.ComputeHash(articleView.WebUrl));
                var bytes = await RequestUtil.CacheFile(articleView.WebUrl, fileName, true);
                var html = Encoding.UTF8.GetString(bytes);
                
                if (!string.IsNullOrEmpty(html))
                {
                    articleView.LoadArticle(html);
                }
            }
        }

        private void LoadArticle(string html)
        {
            var paragraphs = ArticleHelper.AnalyzeHtml(html);
            Paragraphs.Clear();
            foreach (var articleParagraph in paragraphs)
            {
                Paragraphs.Add(articleParagraph);
            }
        }


        public static readonly DependencyProperty TextBrushProperty = DependencyProperty.Register(
            "TextBrush", typeof (Brush), typeof (ArticleView), new PropertyMetadata(default(Brush)));

        public Brush TextBrush
        {
            get { return (Brush) GetValue(TextBrushProperty); }
            set { SetValue(TextBrushProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (ArticleView), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public async void VideoClick(ArticleParagraph paragraph)
        {
            await Launcher.LaunchUriAsync(new Uri(paragraph.Value));
        }

        private void VideoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button) sender;
            VideoClick((ArticleParagraph) btn.DataContext);
        }



        #region Flyout




        private void OnReplyComment(ArticleParagraph e)
        {
            ReplyComment?.Invoke(this, e);
        }
        private void OnReportComment(ArticleParagraph e)
        {
            ReportComment?.Invoke(this, e);
        }
        private void OnDeleteComment(ArticleParagraph e)
        {
            DeleteComment?.Invoke(this, e);
        }

        #endregion


        private void ReplyComment_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem) sender;
            var commentInfo = (ArticleParagraph)item.DataContext;
            OnReplyComment(commentInfo);
        }

        private void ReportComment_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            var commentInfo = (ArticleParagraph)item.DataContext;
            OnReportComment(commentInfo);
        }

        private void DeleteComment_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            var commentInfo = (ArticleParagraph)item.DataContext;
            OnDeleteComment(commentInfo);
        }


        public void GoToTop()
        {
            var sv = VisualTreeUtil.FindVisualChild<ScrollViewer>(listView);
            sv.ChangeView(0, 0, null);
        }


    }
}
