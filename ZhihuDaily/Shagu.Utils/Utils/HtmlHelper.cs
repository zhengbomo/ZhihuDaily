namespace Shagu.Utils.Utils
{
    public class HtmlHelper
    {
        public static string ConvertHtmlText(string text)
        {
            text = WebUtility.HtmlDecode(text);
            return text.Replace("&nbsp;", " ")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&rdquo;", "”")
                .Replace("&amp;", "&")
                .Replace("&ldquo;", "“")
                .Replace("&lsquo;", "‘")
                .Replace("&rsquo;", "’")
                .Replace("&bull;", "·")
                .Replace("&middot;", "·")
                .Replace("&mdash;", "——")
                .Replace("&hellip;", "…")
                .Replace("&cap;", "∩")
                .Replace("&darr;", "↓")
                .Replace("&times;", "×")
               ;
        }
    }
}
