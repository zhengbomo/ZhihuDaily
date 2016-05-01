using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using ZhihuDaily.Controls;
using ZhihuDaily.Models;
using HtmlAgilityPack;
using Shagu.Utils.Utils;

namespace ZhihuDaily.Utils
{
    public class ArticleHelperBase
    {
        protected static void BaseAnalyze(HtmlNode childNode, ICollection<ArticleParagraph> paragraphs)
        {
            switch (childNode.Name)
            {
                case "strong":
                    //文本
                    if (!string.IsNullOrEmpty(childNode.InnerText.Trim()))
                    {
                        paragraphs.Add(new ArticleParagraph
                        {
                            Type = ArticleParagraphType.ArticleParagraphTypeStrongText,
                            Content = HtmlHelper.ConvertHtmlText(childNode.InnerText)
                        });
                    }
                    break;
                case "#text":
                case "span":
                    //文本
                    if (!string.IsNullOrEmpty(childNode.InnerText?.Trim()))
                    {
                        paragraphs.Add(new ArticleParagraph
                        {
                            Type = ArticleParagraphType.ArticleParagraphTypeText,
                            Content = HtmlHelper.ConvertHtmlText(childNode.InnerText)
                        });
                    }
                    break;
                case "a":
                    DoWithA(childNode, paragraphs);
                    break;
                case "br":
                    paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeText,
                        Content = Environment.NewLine
                    });
                    break;
                default:
                    //文本
                    if (!string.IsNullOrEmpty(childNode.InnerText?.Trim()))
                    {
                        paragraphs.Add(new ArticleParagraph
                        {
                            Type = ArticleParagraphType.ArticleParagraphTypeText,
                            Content = HtmlHelper.ConvertHtmlText(childNode.InnerText)
                        });
                    }
                    break;
            }
        }

        protected static Paragraph AnalyzeInline(HtmlNode childNode)
        {
            var paragraph = new Paragraph();
            foreach (var node in childNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "#text":
                    case "span":
                        //文本
                        var text = node.InnerText.Trim();
                        if (!string.IsNullOrEmpty(text))
                        {
                            paragraph.Inlines.Add(new Run {Text = HtmlHelper.ConvertHtmlText(text)});
                        }
                        break;
                    case "hr":
                        paragraph.Inlines.Add(new Run {Text = "————————————————————"});
                        break;
                    case "a":
                        var img = node.ChildNodes.FirstOrDefault(n => n.Name.Equals("img"));
                        if (img != null)
                        {
                            //当成图片处理
                            DoWithImg(img, paragraph);
                        }
                        else
                        {
                            var href = node.GetAttributeValue("href", null);

                            var hyperLink = new Hyperlink();

                            if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                            {
                                hyperLink.NavigateUri = new Uri(href);
                            }
                            hyperLink.Inlines.Add(new Run { Text = HtmlHelper.ConvertHtmlText(node.InnerText) });
                            paragraph.Inlines.Add(hyperLink);
                        }
                        break;
                    case "br":
                        paragraph.Inlines.Add(new LineBreak());
                        break;
                    case "strong":
                        text = node.InnerText.Trim();
                        if (!string.IsNullOrEmpty(text))
                        {
                            var bldText = new Bold();
                            bldText.Inlines.Add(new Run {Text = HtmlHelper.ConvertHtmlText(text)});
                            paragraph.Inlines.Add(bldText);
                        }
                        break;

                    default:
                        text = node.InnerText;
                        if (!string.IsNullOrEmpty(text))
                        {
                            paragraph.Inlines.Add(new Run {Text = HtmlHelper.ConvertHtmlText(text)});
                        }
                        break;
                }
            }
            return paragraph;
        }

        protected static void AnalyzeUl(HtmlNode childNode, ICollection<ArticleParagraph> paragraphs)
        {
            paragraphs.Add(new ArticleParagraph
            {
                Type = ArticleParagraphType.ArticleParagraphTypeEmptyLine,
            });
            foreach (var node in childNode.ChildNodes)
            {
                if (node.Name.Equals("li"))
                {
                    paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeText,
                        Content = " · " + HtmlHelper.ConvertHtmlText(node.InnerText)
                    });
                }
            }
            paragraphs.Add(new ArticleParagraph
            {
                Type = ArticleParagraphType.ArticleParagraphTypeEmptyLine,
            });
        }

        protected static void AnalyzeOl(HtmlNode childNode, ICollection<ArticleParagraph> paragraphs)
        {
            paragraphs.Add(new ArticleParagraph
            {
                Type = ArticleParagraphType.ArticleParagraphTypeEmptyLine,
            });

            int i = 1;
            foreach (var node in childNode.ChildNodes)
            {
                if (node.Name.Equals("li"))
                {
                    paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeText,
                        Content = string.Format("{0}、{1}", i, HtmlHelper.ConvertHtmlText(node.InnerText))
                    });
                    i++;
                }
            }
            paragraphs.Add(new ArticleParagraph
            {
                Type = ArticleParagraphType.ArticleParagraphTypeEmptyLine,
            });
        }

        protected static void DoWithImg(HtmlNode node, ICollection<ArticleParagraph> paragraphs)
        {
            var src = node.GetAttributeValue("src", null) ?? node.GetAttributeValue("data-src", null);
            if (src != null)
            {
                paragraphs.Add(new ArticleParagraph
                {
                    Type = ArticleParagraphType.ArticleParagraphTypeImage,
                    Content = src
                });
            }
        }

        private static void DoWithA(HtmlNode node, ICollection<ArticleParagraph> paragraphs)
        {
            var img = node.ChildNodes.FirstOrDefault(n => n.Name.Equals("img"));
            if (img != null)
            {
                //当成图片处理
                node = img;
                DoWithImg(node, paragraphs);
            }
            else
            {
                var href = node.GetAttributeValue("href", null);
                if (href != null && href.Trim().StartsWith("http"))
                {
                    paragraphs.Add(new ArticleParagraph
                    {
                        Type = ArticleParagraphType.ArticleParagraphTypeLink,
                        Content = HtmlHelper.ConvertHtmlText(node.InnerText),
                        Value = href.Trim()
                    });
                }
            }
        }

        protected static void DoWithImg(HtmlNode node, Paragraph paragraph)
        {
            var src = node.GetAttributeValue("src", null) ?? node.GetAttributeValue("data-src", null);
            if (!string.IsNullOrEmpty(src) && src.StartsWith("http"))
            {
                var container = new InlineUIContainer
                {
                    Child = new ClickableCacheableImage
                    {
                        MinHeight = 300,
                        ImageUrl = src,
                        DefaultImageUri = new Uri("ms-appx:///Assets/Images/default_image.png"),
                        IsShowImageViewer = true,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(0, 5, 0, 5)
                    }
                };
                paragraph.Inlines.Add(container);
            }
        }


        protected static void DoWithA(HtmlNode node, Paragraph paragraph)
        {
            var img = node.ChildNodes.FirstOrDefault(n => n.Name.Equals("img"));
            if (img != null)
            {
                //当成图片处理
                DoWithImg(img, paragraph);
            }
            else
            {
                var href = node.GetAttributeValue("href", null);
                if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                {
                    var hyperLink = new Hyperlink {NavigateUri = new Uri(href)};
                    hyperLink.Inlines.Add(new Run {Text = HtmlHelper.ConvertHtmlText(node.InnerText)});
                    paragraph.Inlines.Add(hyperLink);
                }
            }
        }
    }
}