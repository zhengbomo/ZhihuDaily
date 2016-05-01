using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Shagu.Utils.Utils
{
    public class RequestUtilBase
    {
        /// <summary>
        /// 请求url，并缓存
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="loadNew">是否忽略已存在缓存</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <returns></returns>
        public static async Task<byte[]> CacheFile(string url, string fileName, bool loadNew = false, bool autoRedirect = false)
        {
            var bytes = loadNew ? null : await CacheManagerBase.ReadFileForBytes(fileName);
            if (bytes == null)
            {
                //下载
                try
                {
                    var client = new HttpClient(new HttpClientHandler {AllowAutoRedirect = autoRedirect});
                    bytes = await client.GetByteArrayAsync(url);

                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                        CreationCollisionOption.ReplaceExisting);

                    await FileManager.WriteBytes(file, bytes);
                }
                catch (HttpRequestException e)
                {
                    //网络问题
                    Debug.WriteLine("请求失败:{0}", e);
                    if (loadNew)
                    {
                        //请求失败，使用缓存数据
                        bytes = await CacheManagerBase.ReadFileForBytes(fileName);
                    }
                }
                catch (WebException e)
                {
                    //网络问题
                    Debug.WriteLine("请求失败:{0}", e);
                    if (loadNew)
                    {
                        //请求失败，使用缓存数据
                        bytes = await CacheManagerBase.ReadFileForBytes(fileName);
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    //网络问题
                    Debug.WriteLine("阻止访问:{0}", e);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("请求失败:{0}", e);
                    if (loadNew)
                    {
                        //请求失败，使用缓存数据
                        bytes = await CacheManagerBase.ReadFileForBytes(fileName);
                    }
                }
            }
            return bytes;
        }

        public static async Task<Uri> CacheFileForUri(string url, string fileName, bool loadNew = false, bool autoRedirect = false)
        {
            bool exists = await FileManager.IsFileExists(ApplicationData.Current.LocalFolder, fileName);

            if (!exists || loadNew)
            {
               //不存在，则下载
                try
                {
                    var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = autoRedirect });
                    var bytes = await client.GetByteArrayAsync(url);

                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                        CreationCollisionOption.ReplaceExisting);

                    await FileManager.WriteBytes(file, bytes);
                }
                catch (HttpRequestException e)
                {
                    //网络问题
                    Debug.WriteLine("请求失败:{0}", e);
                }
                catch (WebException e)
                {
                    //网络问题
                    Debug.WriteLine("请求失败:{0}", e);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("请求失败:{0}", e);
                }
            }

            return new Uri(string.Format("ms-appdata:///local/{0}", fileName.Replace("\\", @"/")));
        }
    }
}
