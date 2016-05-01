using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

// Comes from here:
// http://winrtxamltoolkit.codeplex.com/SourceControl/changeset/view/0657c67a93d5#WinRTXamlToolkit/Imaging/WriteableBitmapSaveExtensions.cs

namespace XamlBrewer.Uwp.Controls
{
    public static class WriteableBitmapExtensions
    {
        private static Guid GetEncoderId(string fileName)
        {
            Guid encoderId;

            var ext = Path.GetExtension(fileName);

            if (new[] { ".bmp", ".dib" }.Contains(ext))
            {
                encoderId = BitmapEncoder.BmpEncoderId;
            }
            else if (new[] { ".tiff", ".tif" }.Contains(ext))
            {
                encoderId = BitmapEncoder.TiffEncoderId;
            }
            else if (new[] { ".gif" }.Contains(ext))
            {
                encoderId = BitmapEncoder.TiffEncoderId;
            }
            else if (new[] { ".jpg", ".jpeg", ".jpe", ".jfif", ".jif" }.Contains(ext))
            {
                encoderId = BitmapEncoder.TiffEncoderId;
            }
            else if (new[] { ".hdp", ".jxr", ".wdp" }.Contains(ext))
            {
                encoderId = BitmapEncoder.JpegXREncoderId;
            }
            else //if (new [] {".png"}.Contains(ext))
            {
                encoderId = BitmapEncoder.PngEncoderId;
            }

            return encoderId;
        }

        public static async Task SaveAsync(
            this WriteableBitmap writeableBitmap,
            StorageFile outputFile)
        {
            var encoderId = GetEncoderId(outputFile.Name);

            try
            {
                Stream stream = writeableBitmap.PixelBuffer.AsStream();
                byte[] pixels = new byte[(uint)stream.Length];
                await stream.ReadAsync(pixels, 0, pixels.Length);

                using (var writeStream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(encoderId, writeStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied,
                        (uint)writeableBitmap.PixelWidth,
                        (uint)writeableBitmap.PixelHeight,
                        96,
                        96,
                        pixels);

                    await encoder.FlushAsync();

                    using (var outputStream = writeStream.GetOutputStreamAt(0))
                    {
                        await outputStream.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Your exception handling here..
                throw;
            }
        }

        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            StorageFile storageFile)
        {
            var wb = writeableBitmap;

            using (var stream = await storageFile.OpenReadAsync())
            {
                await wb.SetSourceAsync(stream);
            }

            return wb;
        }
    }
}