using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZhihuDaily.Models;
using ZhihuDaily.Utils;

namespace ZhihuDaily.Util
{
    public class ArticleParagraphTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyLineTemplate { get; set; }
        public DataTemplate H1Template { get; set; }
        public DataTemplate PreTemplate { get; set; }
        public DataTemplate HrLineTemplate { get; set; }
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate TextTemplate { get; set; }

        public DataTemplate AuthorTextTemplate { get; set; }
        public DataTemplate VideoTemplate { get; set; }

        
        public DataTemplate LinkTemplate { get; set; }
        public DataTemplate LiangpingLinkTemplate { get; set; }
        public DataTemplate RichTextTemplate { get; set; }
        public DataTemplate StrongTextTemplate { get; set; }
        public DataTemplate CommentTemplate { get; set; }
        public DataTemplate NoLongCommentTemplate { get; set; }
        public DataTemplate AdTemplate { get; set; }

        public DataTemplate CommentTitleTemplate { get; set; }
        public DataTemplate BlockquoteTemplate { get; set; }

        
        public DataTemplate AvatarTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var model = (ArticleParagraph) item;
            switch (model.Type)
            {
                case ArticleParagraphType.ArticleParagraphTypeAuthor:
                    return AuthorTextTemplate;
                case ArticleParagraphType.ArticleParagraphTypeEmptyLine:
                    return EmptyLineTemplate;
                case ArticleParagraphType.ArticleParagraphTypeH1:
                    return H1Template;
                case ArticleParagraphType.ArticleParagraphTypePre:
                    return PreTemplate;
                case ArticleParagraphType.ArticleParagraphTypeHrLine:
                    return HrLineTemplate;
                case ArticleParagraphType.ArticleParagraphTypeImage:
                    return ImageTemplate;
                case ArticleParagraphType.ArticleParagraphTypeText:
                    return TextTemplate;
                case ArticleParagraphType.ArticleParagraphTypeLink:
                    return LinkTemplate;
                case ArticleParagraphType.ArticleParagraphTypeLiangpingLink:
                    return LiangpingLinkTemplate;
                case ArticleParagraphType.ArticleParagraphTypeRichText:
                    return RichTextTemplate;
                case ArticleParagraphType.ArticleParagraphTypeStrongText:
                    return StrongTextTemplate;
                case ArticleParagraphType.ArticleParagraphTypeVideo:
                    return VideoTemplate;
                case ArticleParagraphType.ArticleParagraphTypeAvatar:
                    return AvatarTemplate;
                case ArticleParagraphType.ArticleParagraphTypeComment:
                    return CommentTemplate;
                case ArticleParagraphType.ArticleParagraphTypeAd:
                    return AdTemplate;
                case ArticleParagraphType.ArticleParagraphTypeCommentTitle:
                    return CommentTitleTemplate;
                case ArticleParagraphType.ArticleParagraphTypeBlockquote:
                    return BlockquoteTemplate;
                case ArticleParagraphType.ArticleParagraphTypeNoLongComment:
                    return NoLongCommentTemplate;
                default:
                    return null;
            }
        }
    }
}
