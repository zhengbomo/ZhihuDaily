using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace ZhihuDaily.Domain.Models
{
    [Table("collection_info")]
    public class CollectionInfo : StoryInfo
    {
        [Column("createTime")]
        public DateTime CreateTime { get; set; }


        [Column("imagesJson")]
        public string ImagesJson
        {
            get { return Images == null ? null : JsonConvert.SerializeObject(Images); }
            set { if (value != null) Images = JsonConvert.DeserializeObject<List<string>>(value); }
        }


        public static CollectionInfo CreateFromStoryInfo(StoryDetailResult storyinfo, string image, bool multipic)
        {
            return new CollectionInfo
            {
                CreateTime = DateTime.Now,
                Images = string.IsNullOrEmpty(image) ? null : new List<string> {image},
                Id = storyinfo.Id,
                Type = storyinfo.Type,
                GaPrefix = storyinfo.GaPrefix,
                Multipic = multipic,
                Title = storyinfo.Title
            };
        }
    }
}