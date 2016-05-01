// *************************************************
// 
// 作者：bomo
// 小组：WP开发组
// 创建日期：2014/5/25 0:49:04
// 版本号：V1.00
// 说明:
// 
// *************************************************
// 
// 修改历史: 
// Date				WhoChanges		Made 
// 10/04/2014 15:23:27		bomo 		Initial creation 
//
// *************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Shagu.Weibo.Social
{
    public abstract class SocialClientBase
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string AppRedirectUrl { get; set; }

        public string RootUrl { get; set; }

        public string AuthorizeUrl { get; set; }
        public string TokenUrl { get; set; }

        //发布文本
        public string PublishUrl { get; set; }

        //发布图文
        public string PublishWithImageUrl { get; set; }

        public abstract List<KeyValuePair<string, string>> GetPublishParams(string token, string content);

        public abstract MultipartFormDataContent GetPublishWithImageParams(string token, string content, Stream stream);

        protected StreamContent CreateImageContent(Stream stream, string fileName, string contentType = "image/jpeg")
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = fileName,
                FileName = Guid.NewGuid() + ".jpg",
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return fileContent;
        }
    }
}
