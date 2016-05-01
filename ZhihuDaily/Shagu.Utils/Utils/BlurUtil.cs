using System;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Shagu.Utils.Utils
{
    public class BlurUtil
    {
        public static async Task Blur(IStorageFile source, IStorageFile target, float blurAmount = 8.0f)
        {
            using (var stream = await source.OpenAsync(FileAccessMode.Read))
            {
                await Blur(stream, target, blurAmount);
            }
        }

        public static async Task Blur(byte[] source, IStorageFile target, float blurAmount = 8.0f)
        {
            using (var stream = StreamUtil.ConvertBytesToIRandomAccessStream(source))
            {
                await Blur(stream, target, blurAmount);
            }
        }

        public static async Task Blur(IRandomAccessStream source, IStorageFile target, float blurAmount = 8.0f)
        {
            int width;
            int height;
            var decoder = await BitmapDecoder.CreateAsync(source);
            var pixelData = await decoder.GetPixelDataAsync();
            var bytes = pixelData.DetachPixelData();
            width = (int)decoder.PixelWidth;
            height = (int)decoder.PixelHeight;

            var device = new CanvasDevice();
            var renderer = new CanvasRenderTarget(device, width, height, 72);
            var bitmap = CanvasBitmap.CreateFromBytes(device, bytes, width, height, DirectXPixelFormat.B8G8R8A8UIntNormalized);
            using (var ds = renderer.CreateDrawingSession())
            {
                var blur = new GaussianBlurEffect
                {
                    BlurAmount = blurAmount,
                    BorderMode = EffectBorderMode.Hard,
                    Optimization = EffectOptimization.Speed,
                    Source = bitmap,
                };
                ds.DrawImage(blur);
            }
            using (var outStream = await target.OpenAsync(FileAccessMode.ReadWrite))
            {
                await renderer.SaveAsync(outStream, CanvasBitmapFileFormat.Png);
            }

            var bmp = new BitmapImage();
            bmp.SetSource(await target.OpenReadAsync());

        }

    }
}
