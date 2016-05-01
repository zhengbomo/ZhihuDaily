using System;
using System.Globalization;
using System.IO;

namespace Shagu.Utils.Utils
{
    public static class WebUtility
    {
        public static void HtmlDecode(string value, TextWriter output)
        {
            if (value == null)
            {
                return;
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            for (var i = 0; i < value.Length; i++)
            {
                var chr = value[i];
                if (chr == '&')
                {
                    if (value[i + 1] == '#')
                    {
                        if (i + 2 < value.Length
                            && char.IsDigit(value[i + 2]))
                        {
                            char chr1 = value[i + 2];
                            if (i + 3 < value.Length)
                            {
                                int num;
                                if (value[i + 3] == ';')
                                {
                                    num = int.Parse(new string(new[] { chr1 }));
                                    if (num > 0 && num < 65536)
                                    {
                                        output.Write((char)num);
                                        i += 3;
                                        continue;
                                    }
                                }
                                else if (char.IsDigit(value[i + 3]))
                                {
                                    char chr2 = value[i + 3];
                                    if (i + 4 < value.Length)
                                    {
                                        if (value[i + 4] == ';')
                                        {
                                            num = int.Parse(new string(new[] { chr1, chr2 }));
                                            if (num > 0 && num < 65536)
                                            {
                                                output.Write((char)num);
                                                i += 4;
                                                continue;
                                            }
                                        }
                                        else if (char.IsDigit(value[i + 4]))
                                        {
                                            char chr3 = value[i + 4];
                                            if (i + 5 < value.Length)
                                            {
                                                if (value[i + 5] == ';')
                                                {
                                                    num = int.Parse(new string(new[] { chr1, chr2, chr3 }));
                                                    if (num > 0 && num < 65536)
                                                    {
                                                        output.Write((char)num);
                                                        i += 5;
                                                        continue;
                                                    }
                                                }
                                                else if (char.IsDigit(value[i + 5]))
                                                {
                                                    char chr4 = value[i + 5];
                                                    if (i + 6 < value.Length)
                                                    {
                                                        if (value[i + 6] == ';')
                                                        {
                                                            num = int.Parse(new string(new[] { chr1, chr2, chr3, chr4 }));
                                                            if (num > 0 && num < 65536)
                                                            {
                                                                output.Write((char)num);
                                                                i += 6;
                                                                continue;
                                                            }
                                                        }
                                                        else if (char.IsDigit(value[i + 6]))
                                                        {
                                                            char chr5 = value[i + 6];
                                                            if (i + 7 < value.Length)
                                                            {
                                                                if (value[i + 7] == ';')
                                                                {
                                                                    num = int.Parse(new string(new[] { chr1, chr2, chr3, chr4, chr5 }));
                                                                    if (num > 0 && num < 65536)
                                                                    {
                                                                        output.Write((char)num);
                                                                        i += 7;
                                                                        continue;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (value.IndexOf("quot;", i + 1, StringComparison.Ordinal) == i + 1)
                    {
                        output.Write('\"');
                        i += 5;
                    }
                    else if (value.IndexOf("amp;", i + 1, StringComparison.Ordinal) == i + 1)
                    {
                        output.Write('&');
                        i += 4;
                    }
                    else if (value.IndexOf("lt;", i + 1, StringComparison.Ordinal) == i + 1)
                    {
                        output.Write('<');
                        i += 3;
                    }
                    else if (value.IndexOf("gt;", i + 1, StringComparison.Ordinal) == i + 1)
                    {
                        output.Write('>');
                        i += 3;
                    }
                    else
                    {
                        output.Write(chr);
                    }
                }
                else
                {
                    output.Write(chr);
                }
            }
        }

        public static string HtmlDecode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            var output = new StringWriter(CultureInfo.InvariantCulture);
            HtmlDecode(value, output);
            return output.ToString();
        }

        public static void HtmlEncode(string value, TextWriter output)
        {
            if (value == null)
            {
                return;
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            foreach (char chr in value)
            {
                if (chr >= 160 && chr <= 255)
                {
                    output.Write("&#" + Convert.ToInt32(chr) + ";");
                    continue;
                }
                switch (chr)
                {
                    case '\"':
                        output.Write("&quot;");
                        break;

                    case '&':
                        output.Write("&amp;");
                        break;

                    case '\'':
                        output.Write("&#39;");
                        break;

                    case '<':
                        output.Write("&lt;");
                        break;

                    case '>':
                        output.Write("&gt;");
                        break;

                    default:
                        output.Write(chr);
                        break;
                }
            }
        }

        public static string HtmlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            var output = new StringWriter(CultureInfo.InvariantCulture);
            HtmlEncode(value, output);
            return output.ToString();
        }

        public static string UrlEncode(string value)
        {
            if (value == null)
            {
                return null;
            }

            return Uri.EscapeDataString(value);
        }

        public static string UrlDecode(string value)
        {
            if (value == null)
            {
                return null;
            }

            value = value.Replace('+', ' ');
            return Uri.UnescapeDataString(value);
        }
    }
}