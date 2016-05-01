using Windows.UI.Xaml.Controls;

namespace ZhihuDaily.Models
{
    public enum ArticleParagraphType
    {
        ArticleParagraphTypeText,
        ArticleParagraphTypeImage,
        ArticleParagraphTypeRichText,
        ArticleParagraphTypeLink,
        ArticleParagraphTypeLiangpingLink,
        ArticleParagraphTypeHrLine,

        //引用
        ArticleParagraphTypeBlockquote,
        //标题
        ArticleParagraphTypeH1,
        //引用
        ArticleParagraphTypePre,
        //作者信息
        ArticleParagraphTypeAuthor,
        //空行
        ArticleParagraphTypeEmptyLine,
        //加粗
        ArticleParagraphTypeStrongText,

        //视频
        ArticleParagraphTypeVideo,

        //头像
        ArticleParagraphTypeAvatar,

        //没有长评论
        ArticleParagraphTypeNoLongComment,
        //评论
        ArticleParagraphTypeComment,

        //广告
        ArticleParagraphTypeAd,

        //评论标题
        ArticleParagraphTypeCommentTitle,


    }


    public class ArticleParagraph
    {
        public ArticleParagraphType Type { get; set; }

        public string Content { get; set; }
        public string Value { get; set; }
        public string SubValue { get; set; }

        public object TagValue { get; set; }
        public RichTextBlock RichTextBlock { get; set; }
    }
}
