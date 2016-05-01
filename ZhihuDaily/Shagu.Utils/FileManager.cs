using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Shagu.Utils
{
    public class FileManager
    {
        private static readonly SemaphoreSlim ReadLock = new SemaphoreSlim(1, 1);

        public static async Task<IStorageFile> GetFile(StorageFolder folder, string fileName)
        {
            return await folder.GetFileAsync(fileName);
        }
    
        public static async Task LockWriteText(IStorageFile file, string text)
        {
            try
            {
                await ReadLock.WaitAsync();
                await FileIO.WriteTextAsync(file, text);
                Debug.WriteLine("lockwriteFile");
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.LockWriteText faild:{0}", e);
            }
            finally
            {
                ReadLock.Release();
            }
        }

        public static async Task WriteText(IStorageFile file, string text)
        {
            try
            {
                await FileIO.WriteTextAsync(file, text);
                Debug.WriteLine("writeFile");
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.WriteText faild:{0}", e);
            }
        }

        public static async Task LockWriteBytes(IStorageFile file, byte[] bytes)
        {
            try
            {
                await ReadLock.WaitAsync();
                await FileIO.WriteBytesAsync(file, bytes);
                Debug.WriteLine("lockwriteFile");
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.LockWriteBytes faild:{0}", e);
            }
            finally
            {
                ReadLock.Release();
            }
        }

        public static async Task WriteBytes(IStorageFile file, byte[] bytes)
        {
            try
            {
                await FileIO.WriteBytesAsync(file, bytes);
                Debug.WriteLine("writeFile");
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.WriteBytes faild:{0}", e);
            }
        }

        public static async Task<string> ReadText(IStorageFile file)
        {
            string text = null;
            try
            {
                text = await FileIO.ReadTextAsync(file);
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.ReadBytes faild:{0}", e);
            }
            return text;
        }

        public static async Task<string> ReadText(StorageFolder folder, string fileName)
        {
            if (await IsFileExists(folder, fileName))
            {
                string text = null;
                try
                {
                    var file = await folder.GetFileAsync(fileName);
                    text = await FileIO.ReadTextAsync(file);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("FileManager.ReadBytes faild:{0}", e);
                }
                return text;
            }
            else
            {
                return null;
            }

        }


        public static async Task<byte[]> ReadBytes(IStorageFile file)
        {
            byte[] bytes = null;
            try
            {
                var buffer = await FileIO.ReadBufferAsync(file);
                if (buffer != null && buffer.Length > 0)
                {
                    bytes = buffer.ToArray(0, (int) buffer.Length);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.ReadBytes faild:{0}", e);
            }
            return bytes;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName">eg: pictures\\mypicture.png</param>
        /// <returns></returns>
        public static async Task<byte[]> ReadBytes(StorageFolder folder, string fileName)
        {
            if (await IsFileExists(folder, fileName))
            {
                byte[] bytes = null;
                try
                {
                    var file = await folder.GetFileAsync(fileName);

                    var buffer = await FileIO.ReadBufferAsync(file);
                    bytes = buffer.ToArray(0, (int) buffer.Length);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("FileManager.ReadBytes faild:{0}", e);
                }
                return bytes;
            }
            else
            {
                return null;
            }
        }
        
        
        public static async Task<bool> IsFileExists(StorageFolder folder, string fileName)
        {
            try
            {
                var item = await folder.TryGetItemAsync(fileName);
                return item != null;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<BitmapImage> ReadBitmapImage(IStorageFile file)
        {
            BitmapImage image = null;
            try
            {
                var buffer = await FileIO.ReadBufferAsync(file);

                using (var iRandomAccessStream = new InMemoryRandomAccessStream())
                {
                    await iRandomAccessStream.WriteAsync(buffer);
                    iRandomAccessStream.Seek(0);
                    image = new BitmapImage();
                    await image.SetSourceAsync(iRandomAccessStream);
                }
                    
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileManager.ReadBytes faild:{0}", e);
            }
            return image;
        }



        public static async Task LockDeleteFolderAsync(IStorageFolder folder)
        {
            try
            {
                await ReadLock.WaitAsync();
                await deleteFolderAsync(folder);
            }
            catch (Exception e)
            {
                Debug.WriteLine("LockDeleteFolderAsync faild:{0}", e);
            }
            finally
            {
                ReadLock.Release();
            }
        }

        public static async Task<ulong> GetFolderSizeAsync(IStorageFolder folder)
        {
            ulong size = 0;
            try
            {
                await ReadLock.WaitAsync();
                size = await getFolderSizeAsync(folder);
            }
            catch (Exception e)
            {
                Debug.WriteLine("LockDeleteFolderAsync faild:{0}", e);
            }
            finally
            {
                ReadLock.Release();
            }
            return size;
        }
        
        

        public static string GetSizeString(ulong size)
        {
            const double promoteLimit = 1024.0;
            const double decimalLimit = 10.0;
            const string separator = " ";

            if (size < promoteLimit)
            {
                return string.Format("{0}{1} B", new object[]
                {
                    size,
                    separator
                });
            }
            double num = size / 1024.0;
            if (num < decimalLimit)
            {
                return string.Format("{0:F1}{1} KB", new object[]
                {
                    num,
                    separator
                });
            }
            if (num < promoteLimit)
            {
                return string.Format("{0:F0}{1} KB", new object[]
                {
                    num,
                    separator
                });
            }
            double num2 = num / 1024.0;
            if (num2 < decimalLimit)
            {
                return string.Format("{0:F1}{1} MB", new object[]
                {
                    num2,
                    separator
                });
            }
            if (num2 < promoteLimit)
            {
                return string.Format("{0:F0}{1} MB", new object[]
                {
                    num2,
                    separator
                });
            }
            double num3 = num2 / 1024.0;
            if (num3 < decimalLimit)
            {
                return string.Format("{0:F1}{1} GB", new object[]
                {
                    num3,
                    separator
                });
            }
            if (num3 < promoteLimit)
            {
                return string.Format("{0:F0}{1} GB", new object[]
                {
                    num3,
                    separator
                });
            }
            double num4 = num3 / 1024.0;
            if (num4 < decimalLimit)
            {
                return string.Format("{0:F1}{1} TB", new object[]
                {
                    num4,
                    separator
                });
            }
            return string.Format("{0:F0}{1} TB", new object[]
            {
                num4,
                separator
            });
        }

        #region Private Method

        private static async Task deleteFolderAsync(IStorageFolder folder)
        {
            var folders = await folder.GetFoldersAsync();
            foreach (var storageFolder in folders)
            {
                await deleteFolderAsync(storageFolder);
            }

            var files = await folder.GetFilesAsync();
            await files.First().DeleteAsync();
            foreach (var storageFile in files)
            {
                await storageFile.DeleteAsync();
            }
        }

        public static async Task<ulong> getFolderSizeAsync(IStorageFolder folder)
        {
            ulong size = 0;

            var folders = await folder.GetFoldersAsync();
            foreach (var storageFolder in folders)
            {
                size += await getFolderSizeAsync(storageFolder);
            }
            var files = await folder.GetFilesAsync();
            foreach (var storageFile in files)
            {
                var property = await storageFile.GetBasicPropertiesAsync();
                size += property.Size;
            }

            return size;
        }

        #endregion

    }
}
