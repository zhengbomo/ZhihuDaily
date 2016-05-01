using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Image = Windows.UI.Xaml.Controls.Image;

namespace Shagu.Utils.Utils
{
    public static class ImageUtil
    {
        /// <summary>
        /// 缓存地址：cacheImage\\
        /// </summary>
        public static string CacheFolder { get; set; }

        static ImageUtil()
        {
            CacheFolder = "cacheImage\\";
        }

        public static async Task LoadImage(Image image, Uri defaultImageUri, string url, bool loadNew, Action<byte[]> callback, Action failActon = null)
        {
            if (defaultImageUri != null)
            {
                image.Source = new BitmapImage(defaultImageUri);
            }
            await LoadImage(url, loadNew, callback, failActon);
        }

        public static async Task LoadImage(Shape path, Uri defaultImageUri, string url, bool loadNew, Action<byte[]> callback, Action failActon = null)
        {
            if (defaultImageUri != null)
            {
                var imageBrush = new ImageBrush {ImageSource = new BitmapImage(defaultImageUri)};
                path.Fill = imageBrush;
            }
            await LoadImage(url, loadNew, callback, failActon);
        }

        private static async Task LoadImage(string url, bool loadNew, Action<byte[]> callback, Action failActon)
        {
            url = url?.Trim();
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http"))
            {
                var fileName = string.Format("{0}{1}", CacheFolder, Md5Helper.ComputeHash(url));
                var bytes = await RequestUtilBase.CacheFile(url, fileName, loadNew);
                if (bytes != null && bytes.Length > 0)
                {
                    callback(bytes);
                }
                else
                {
                    failActon?.Invoke();
                }
            }
        }

        public static async Task<Uri> LoadImage(string url, bool loadNew)
        {
            url = url?.Trim();
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http"))
            {
                var fileName = string.Format("{0}{1}", CacheFolder, Md5Helper.ComputeHash(url));
                return await RequestUtilBase.CacheFileForUri(url, fileName, loadNew);
            }
            return null;
        }


        public static async Task<WriteableBitmap> LoadImage(byte[] data)
        {
            using (Stream stream = new MemoryStream(data))
            {
                var writeableBitmap = new WriteableBitmap(1, 1);
                return await writeableBitmap.FromStream(stream);
            }
        }
    }
}
