using System;
using Windows.Web.Http;

namespace ZhihuDaily.Views
{
    public sealed partial class WebBrowserView 
    {
        public WebBrowserView()
        {
            InitializeComponent();

            string ua = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X)" + "AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25";
            Uri targetUri = new Uri("http://v.youku.com/v_show/id_XODI1NTk5NzM2.html");
            var hrm = new HttpRequestMessage(HttpMethod.Get, targetUri);
            hrm.Headers.Add("User-Agent", ua);
            webView.NavigateWithHttpRequestMessage(hrm);
        }
    }
}
