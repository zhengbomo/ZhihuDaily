using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace Shagu.Utils.Utils
{
    public class CacheManagerBase
    {


        #region Helper

        protected static async Task SaveFile(string path, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            await SaveFile(path, json);
        }

        public static async Task SaveFile(string path, string value)
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                await FileManager.WriteText(file, value);
            }
            catch (JsonException)
            {
                Debug.WriteLine("Json序列化失败");
            }
            catch (Exception)
            {
                Debug.WriteLine("发生其他错误");
            }
        }

        public static async Task Delete(string path)
        {
            try
            {

                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(path);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception)
            {
                Debug.WriteLine("发生其他错误");
            }
        }

        protected static async Task<T> ReadFile<T>(string path, T defaultValue)
        {
            var json = await ReadFile(path);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<T>(json);

            }
            else
            {
                return defaultValue;
            }
        }

        public static async Task<string> ReadFile(string path)
        {
            try
            {
                return await FileManager.ReadText(ApplicationData.Current.LocalFolder, path);
            }

            catch (FileNotFoundException)
            {
                Debug.WriteLine("文件未找到");
            }
            catch (JsonException)
            {
                Debug.WriteLine("Json解析失败");
            }
            catch (Exception)
            {
                Debug.WriteLine("发生其他错误");
            }
            return null;
        }

        public static async Task<byte[]> ReadFileForBytes(string path)
        {
            return await FileManager.ReadBytes(ApplicationData.Current.LocalFolder, path);
        }

        public static async Task<Uri> GetUriWithPath(string fileName, bool isCheck = true)
        {
            if (!isCheck || await FileManager.IsFileExists(ApplicationData.Current.LocalFolder, fileName))
            {
                return new Uri(string.Format("ms-appdata:///local/{0}", fileName.Replace("\\", @"/")));
            }
            else
            {
                return null;
            }
        }

        public static async Task WriteFileForBytes(string path, byte[] data)
        {
            var file = await
                ApplicationData.Current.LocalFolder.CreateFileAsync(path,
                    CreationCollisionOption.ReplaceExisting);

            await FileManager.WriteBytes(file, data);
        }

        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        private static byte[] StreamToBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        #endregion
    }
}
