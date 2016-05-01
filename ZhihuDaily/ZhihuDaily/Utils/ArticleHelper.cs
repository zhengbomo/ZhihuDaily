using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using ZhihuDaily.Models;
using HtmlAgilityPack;
using Shagu.Utils.Utils;
using ZhihuDaily.Util;

namespace ZhihuDaily.Utils
{
    public class ArticleHelper : ArticleHelperBase
    {



        public static double LineHeight => IoC.Get<GlobalInfoManager>().ArticleLineHeight;

        public static List<ArticleParagraph> AnalyzeHtml(string html)
        {
            var paragraphs = new List<ArticleParagraph>();

            if (html != null)
            {
                html = Regex.Replace(html, @"\s+", " ");

                try
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    Analyze(doc.DocumentNode, paragraphs);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

            }
            return paragraphs;
        }

        private static void Analyze(HtmlNode rootNode, ICollection<ArticleParagraph> paragraphs)
        {
            foreach (var childNode in rootNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "div":

                        //处理头像
                        if (childNode.GetAttributeValue("class", string.Empty).Equals("meta"))
                        {
                            //找出头像
                            var avatarImg = childNode.ChildNodes.FirstOrDefault(
                                n => n.Name.Equals("img") && n.GetAttributeValue("class", string.Empty).Equals("avatar"));
                            var author = childNode.ChildNodes.FirstOrDefault(
                               n => n.Name.Equals("span") && n.GetAttributeValue("class", string.Empty).Equals("author"));
                            var bio = childNode.ChildNodes.FirstOrDefault(
                              n => n.Name.Equals("span") && n.GetAttributeValue("class", string.Empty).Equals("bio"));

                            if (avatarImg != null && author != null)
                            {
                                paragraphs.Add(new ArticleParagraph
                                {
                                    Type = ArticleParagraphType.ArticleParagraphTypeAvatar,
                                    TagValue = new AvatarInfo
                                    {
                                        Name = author.InnerText.Trim(),
                                        SubName = bio?.InnerText.Trim(),
                                        Avatar = avatarImg.GetAttributeValue("src", string.Empty)
                                    }
                                });
                                break;
                            }
                        }
                        Analyze(childNode, paragraphs);

                        break;
                    case "section":
                    case "article":
                        Analyze(childNode, paragraphs);
                        break;

                    case "p":
                        DoWithP(childNode, paragraphs);
                        break;


                    case "h1":
                    case "h2":
                    case "h3":
                    case "h4":
                    case "h5":
                    case "h6":
                    case "h7":
                        paragraphs.Add(new ArticleParagraph
                        {
                            Type = ArticleParagraphType.ArticleParagraphTypeH1,
                            Content = HtmlHelper.ConvertHtmlText(childNode.InnerText)
                        });
                        break;
                    case "pre":
                        paragraphs.Add(new ArticleParagraph
                        {
                            Type = ArticleParagraphType.ArticleParagraphTypePre,
                            Content = HtmlHelper.ConvertHtmlText(childNode.InnerText)
                        });
                        break;
                    case "hr":
                        paragraphs.Add(new ArticleParagraph { Type = ArticleParagraphType.ArticleParagraphTypeHrLine });
                        break;
                    case "ul":
                        AnalyzeUl(childNode, paragraphs);
                        break;
                    case "ol":
                        AnalyzeOl(childNode, paragraphs);
                        break;
                    case "img":
                        DoWithImg(childNode, paragraphs);
                        break;
                    case "table":
                        foreach (var node in childNode.ChildNodes)
                        {
                            switch (node.Name)
                            {
                                case "tbody":
                                    foreach (var child in node.ChildNodes)
                                    {
                                        switch (child.Name)
                                        {
                                            case "tr":
                                                paragraphs.Add(new ArticleParagraph
                                                {
                                                    Type = ArticleParagraphType.ArticleParagraphTypeText,
                                                    Content = HtmlHelper.ConvertHtmlText(child.InnerText)
                                                });
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    case "zxvideo":
                        var src = childNode.GetAttributeValue("data-origin", string.Empty);
                        if (!string.IsNullOrEmpty(src))
                        {
                            var imgSrc = string.Empty;
                            //获取图片
                            var img = childNode.ChildNodes.FirstOrDefault(n => n.Name.Equals("img"));
                            if (img!=null)
                            {
                                imgSrc = img.GetAttributeValue("src", string.Empty);
                            }

                            paragraphs.Add(new ArticleParagraph
                            {
                                Type = ArticleParagraphType.ArticleParagraphTypeVideo,
                                Value = src,
                                SubValue = imgSrc,
                            });
                        }
                        break;

                        

                    case "blockquote":
                        var paragraph = AnalyzeInline(childNode);
                        if (paragraph.Inlines.Count > 0)
                        {
                            var richTextBlock = new RichTextBlock {LineHeight = LineHeight };
                            richTextBlock.Blocks.Add(paragraph);

                            paragraphs.Add(new ArticleParagraph
                            {
                                Type = ArticleParagraphType.ArticleParagraphTypeBlockquote,
                                RichTextBlock = richTextBlock
                            });
                        }
                        break;
                    default:
                        BaseAnalyze(childNode, paragraphs);
                        break;

                }
            }
        }


        private static void DoWithP(HtmlNode rootNode, ICollection<ArticleParagraph> paragraphs)
        {
            if (rootNode.ChildNodes.Count <= 1 ||
                //不包含下面
                rootNode.ChildNodes.Any(n =>
                    (n.Name.Equals("div") ||
                     n.Name.Equals("section") ||
                     n.Name.Equals("article") ||
                     n.Name.Equals("p") ||
                     n.Name.Equals("h1") ||
                     n.Name.Equals("h2") ||
                     n.Name.Equals("h3") ||
                     n.Name.Equals("h4") ||
                     n.Name.Equals("h5") ||
                     n.Name.Equals("h6") ||
                     n.Name.Equals("h7") ||
                     n.Name.Equals("pre") ||
                     n.Name.Equals("hr") ||
                     n.Name.Equals("ul") ||
                     n.Name.Equals("ol") ||
                     n.Name.Equals("img") ||
                     n.Name.Equals("zxvideo") ||
                     n.Name.Equals("table"))))
            {
                Analyze(rootNode, paragraphs);
            }
            else
            {
                var paragraph = AnalyzeInline(rootNode);
                if (paragraph.Inlines.Count > 0)
                {
                    var pClass = rootNode.GetAttributeValue("class", null);

                    var richTextBlock = new RichTextBlock { LineHeight = LineHeight };
                    richTextBlock.Blocks.Add(paragraph);

                    switch (pClass)
                    {
                        case "author":
                            paragraphs.Add(new ArticleParagraph
                            {
                                Type = ArticleParagraphType.ArticleParagraphTypeAuthor,
                                RichTextBlock = richTextBlock
                            });
                            break;
                        default:
                            paragraphs.Add(new ArticleParagraph
                            {
                                Type = ArticleParagraphType.ArticleParagraphTypeRichText,
                                RichTextBlock = richTextBlock
                            });
                            break;
                    }
                }
            }
        }
    }
}