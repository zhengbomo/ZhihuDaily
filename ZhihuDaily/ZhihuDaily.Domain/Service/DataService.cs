using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Shagu.Utils.Extends;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Models;

namespace ZhihuDaily.Domain.Service
{
    public class DataService
    {
        public static string DeviceId { get; set; }


        //获取分类
        public static async Task<RequestResult<CategoryResult>> GetCategory()
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/themes");
            return await Get<CategoryResult>(url);
        }

        //获取推荐
        public static async Task<RequestResult<RecommendStoryResult>> GetRecommendStory()
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/stories/latest?client=0");
            return await Get<RecommendStoryResult>(url);
        }

        //获取分类文章
        public static async Task<RequestResult<CategoryDetailResult>> GetCategoryDetail(int categoryId, int storyId)
        {
            var url = storyId == 0 ? string.Format("http://news-at.zhihu.com/api/4/theme/{0}", categoryId) : string.Format("http://news-at.zhihu.com/api/4/theme/{0}/before/{1}", categoryId, storyId);
            return await Get<CategoryDetailResult>(url);
        }

        //获取主列表
        public static async Task<RequestResult<StoryListResult>> GetHomeStory(string date)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/stories/before/{0}?client=0", date);
            return await Get<StoryListResult>(url);
        }

        //获取文章
        public static async Task<RequestResult<StoryDetailResult>> GetStoryDetail(string auth, int storyId)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/story/{0}", storyId);

            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Get<StoryDetailResult>(url, header);
        }

        //获取文章
        public static async Task<RequestResult<CounterList>> GetCounterList(string auth, int storyId)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/story-extra/{0}", storyId);
            var header = new Dictionary<string, string> { { "Authorization", string.Format("Bearer {0}", auth) } };
            return await Get<CounterList>(url, header);
        }


        //获取欢迎页
        public static async Task<RequestResult<StartImageResult>> GetStartImage()
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/start-image/640*1136?client=0");
            return await Get<StartImageResult>(url);
        }

        //获取文章评论
        public static async Task<RequestResult<CommentResult>> GetStoryComment(string auth, int storyId, bool isLongComment, int beforeId)
        {
            string url;
            if (isLongComment)
            {
                url = string.Format("http://news-at.zhihu.com/api/4/story/{0}/long-comments", storyId);
            }
            else
            {
                if (beforeId.Equals(0))
                {
                    url = string.Format("http://news-at.zhihu.com/api/4/story/{0}/short-comments", storyId);
                }
                else
                {
                    url = string.Format("http://news-at.zhihu.com/api/4/story/{0}/short-comments/before/{1}", storyId,
                        beforeId);
                }
            }
            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Get<CommentResult>(url, header);
        }

        //微博登陆
        public static async Task<RequestResult<LoginResult>> Login(string refreshToken, long expiresIn, string accessToken, string userId, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/login");
            //POST
            var postString =
                $"{{\"refresh_token\":\"{refreshToken}\",\"source\":\"sina\",\"expires_in\":{expiresIn},\"access_token\":\"{accessToken}\",\"user\":\"{userId}\"}}";
            HttpContent content = new StringContent(postString);
            return await Post<LoginResult>(url, content, null, token);
        }

        //获取收藏
        public static async Task<RequestResult<FavoriteResult>> GetFavorite(string auth, long lastTime)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/favorites/{0}",
                lastTime == 0 ? string.Empty : string.Format("before/{0}", lastTime));

            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};

            return await Get<FavoriteResult>(url, header);
        }

        //设置收藏
        public static async Task<RequestResult<object>> SetFavorite(string auth, long id, bool favorite)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/favorites");
          
            HttpContent content = new StringContent(string.Format("[{0},{1}]", id, favorite ? 1 : 0));
            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Post<object>(url, content, header);
        }

        //点赞
        public static async Task<RequestResult<object>> SetSupport(string auth, long id, bool support)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/vote/stories");

            HttpContent content = new StringContent(string.Format("[{0},{1}]", id, support ? 1 : 0));
            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Post<object>(url, content, header);
        }

        //改名
        public static async Task<RequestResult<object>> SetName(string auth, string name, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/name");

            HttpContent content = new StringContent(string.Format("{{\"name\":\"{0}\"}}", name));
            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Post<object>(url, content, header, token);
        }

        //该头像
        public static async Task<RequestResult<UserInfo>> SetAvatar(string auth, IRandomAccessStream avatar, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/avatar");
            HttpContent content = new StreamContent(avatar.AsStreamForRead());
            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Post<UserInfo>(url, content, header, token);
        }

        private class AddCommentArg
        {
            [JsonProperty("share_to")]
            public string ShareTo { get; set; }
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("news_id")]
            public int NewsId { get; set; }
            [JsonProperty("reply_to")]
            public int ReplyTo { get; set; }
            
        }

        //写评论
        public static async Task<RequestResult<object>> AddComment(string auth, int storyId, int replyTo, bool share2Sina, string content)
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/story/{0}/comment", storyId);

            var args = new AddCommentArg
            {
                Content = content,
                NewsId = storyId,
                ReplyTo = replyTo,
                ShareTo = share2Sina ? "sina" : string.Empty
            };
            
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(args));

            var header = new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", auth)}};
            return await Post<object>(url, httpContent, header);
        }

        //赞评论
        public static async Task<RequestResult<object>> SetSupport(string auth, int commentId, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/vote/comment/{0}", commentId);
            HttpContent content = new StringContent(string.Format("{{\"comment_id\":\"{0}\"}}", commentId));
            var header = new Dictionary<string, string> { { "Authorization", string.Format("Bearer {0}", auth) } };
            return await Post<object>(url, content, header, token);
        }

        //取消赞
        public static async Task<RequestResult<object>>DelSupport(string auth, int commentId, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/vote/comment/{0}", commentId);
            var header = new Dictionary<string, string> { { "Authorization", string.Format("Bearer {0}", auth) } };
            return await Delete<object>(url, header, token);
        }


        //删除评论
        public static async Task<RequestResult<object>> DelComment(string auth, int commentId, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/comment/{0}", commentId);
            var header = new Dictionary<string, string> { { "Authorization", string.Format("Bearer {0}", auth) } };
            return await Delete<object>(url, header, token);
        }

        //举报评论
        public static async Task<RequestResult<object>> ReportComment(string auth, int commentId, CancellationToken token = new CancellationToken())
        {
            var url = string.Format("http://news-at.zhihu.com/api/4/report/comment/{0}", commentId);
            var header = new Dictionary<string, string> { { "Authorization", string.Format("Bearer {0}", auth) } };
            HttpContent content = new StringContent(string.Format("{{\"comment_id\":\"{0}\"}}", commentId));
            return await Post<object>(url, content, header, token);
        }


        #region Private Method

        private static async Task<RequestResult<T>> Get<T>(string url, Dictionary<string, string> header = null)
        {
            return await Get(url, stream =>
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }, header);
        }

        private static async Task<RequestResult<T>> Get<T>(string url, Func<Stream, T> func, Dictionary<string, string> header = null,
            CancellationToken token = new CancellationToken())
        {
            using (var http = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }))
            {
                try
                {
                    if (header != null)
                    {
                        foreach (var key in header.Keys)
                        {
                            http.DefaultRequestHeaders.Add(key, header[key]);
                        }
                    }

                    http.DefaultRequestHeaders.Add("X-UUID", DeviceId);
                    http.DefaultRequestHeaders.Add("X-App-Version", "2.5.3");
                    http.DefaultRequestHeaders.Add("User-Agent", "daily/201507131700 CFNetwork/711.1.16 Darwin/14.0.0");
                    http.DefaultRequestHeaders.Add("X-Bundle-ID", "com.zhihu.daily");
                    http.DefaultRequestHeaders.Add("X-Api-Version", "4");
                    http.DefaultRequestHeaders.Add("X-Device", "iPhone6,2/N53AP");

                    var response = await http.GetAsync(url, token);

                    token.ThrowIfCancellationRequested();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        return new RequestResult<T>
                        {
                            ResultType = RequestResultType.Success,
                            Content = func(stream)
                        };
                    }
                }
                catch (XmlException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.JsonError,
                    };
                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Cancel,
                        ErrorMessage = Constants.UserCancelOperation,
                    };
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.NetworkError,
                    };
                }
                catch (WebException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.NetworkError,
                    };
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.UnknownError,
                    };
                }
            }
        }

        public static TimeSpan Span { get; set; }

        public static double GetJavaTime(DateTime time)
        {
            return (time + Span - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }


        private static async Task<RequestResult<T>> Post<T>(string url, Dictionary<string, string> param)
        {
            var content = new FormUrlEncodedContent(param ?? new Dictionary<string, string>());
            return await Post<T>(url, content);
        }

        public static async Task<RequestResult<T>> Post<T>(string url, HttpContent content, Dictionary<string, string> header = null, CancellationToken token = new CancellationToken())
        {
            //设置HttpClientHandler的AutomaticDecompression
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                MaxRequestContentBufferSize = int.MaxValue
            };

            //创建HttpClient（注意传入HttpClientHandler）
            using (var http = new HttpClient(handler))
            {
                //await异步等待回应
                try
                {
                    if (header != null)
                    {
                        foreach (var key in header.Keys)
                        {
                            http.DefaultRequestHeaders.Add(key, header[key]);
                        }
                    }

                    var response = await http.PostAsync(url, content, token);

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine(result);

                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Success,
                        Content = JsonConvert.DeserializeObject<T>(result),
                    };

                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Cancel,
                        ErrorMessage = Constants.UserCancelOperation,
                    };
                }
                catch (JsonException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.JsonError,
                    };
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.NetworkError,
                    };
                }
                catch (WebException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.NetworkError,
                    };
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.UnknownError,
                    };
                }
            }
        }


        public static async Task<RequestResult<T>> Delete<T>(string url, Dictionary<string, string> header = null, CancellationToken token = new CancellationToken())
        {
            //设置HttpClientHandler的AutomaticDecompression
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                MaxRequestContentBufferSize = int.MaxValue
            };

            //创建HttpClient（注意传入HttpClientHandler）
            using (var http = new HttpClient(handler))
            {
                //await异步等待回应
                try
                {
                    if (header != null)
                    {
                        foreach (var key in header.Keys)
                        {
                            http.DefaultRequestHeaders.Add(key, header[key]);
                        }
                    }

                    var response = await http.DeleteAsync(url, token);

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine(result);

                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Success,
                        Content = JsonConvert.DeserializeObject<T>(result),
                    };

                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine(e);
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Cancel,
                        ErrorMessage = Constants.UserCancelOperation,
                    };
                }
                catch (JsonException)
                {
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.JsonError,
                    };
                }
                catch (HttpRequestException)
                {
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.NetworkError,
                    };
                }
                catch (WebException)
                {
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.NetworkError,
                    };
                }
                catch (Exception)
                {
                    return new RequestResult<T>
                    {
                        ResultType = RequestResultType.Error,
                        ErrorMessage = Constants.UnknownError,
                    };
                }
            }
        }


        #endregion
    }
}
