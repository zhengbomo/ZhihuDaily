using Caliburn.Micro;
using Shagu.UI.Utils;

namespace ZhihuDaily.ViewModels
{
    public class WebBrowserViewModel : Screen
    {
        public string Title { get; set; }
        public string Url { get; set; }

        private readonly IFrameManager _frameManager;
        public WebBrowserViewModel(IFrameManager frameManager)
        {
            _frameManager = frameManager;
        }

      

    }
}