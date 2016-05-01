using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Shagu.Utils.Utils
{
    public class StreamUtil
    {
        public static async Task<InMemoryRandomAccessStream> ConvertBytesToMemoryRandomAccessStream(byte[] arr)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            await randomAccessStream.WriteAsync(arr.AsBuffer());
            randomAccessStream.Seek(0);
            return randomAccessStream;

            //            MemoryStream stream = new MemoryStream(arr);
            //            return stream.AsRandomAccessStream();
        }

        public static IRandomAccessStream ConvertBytesToIRandomAccessStream(byte[] arr)
        {
            return new MemoryStream(arr).AsRandomAccessStream();
        }
        public static byte[] ConvertMemoryRandomAccessStreamToBytes(InMemoryRandomAccessStream stream)
        {
            var data = new byte[stream.Size];
            var length = stream.AsStreamForRead(0).Read(data, 0, data.Length);
            return data;
        }

    }
}
