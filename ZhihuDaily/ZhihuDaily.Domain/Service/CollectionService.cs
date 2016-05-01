using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using ZhihuDaily.Domain.Models;
using SQLite.Net;
using SQLite.Net.Interop;

namespace ZhihuDaily.Domain.Service
{
    public class CollectionService
    {
        private const string DbPath = "collection_article.db";
        private readonly ISQLitePlatform _sqLitePlatform;

        private readonly string _dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbPath);

        public CollectionService(ISQLitePlatform sqLitePlatform)
        {
            this._sqLitePlatform = sqLitePlatform;

            var db = new SQLiteConnectionWithLock(sqLitePlatform, new SQLiteConnectionString(_dbPath, true));
            using (db.Lock())
            {
                db.CreateTable<CollectionInfo>();
            }
        }
        
        public async Task<StoryDetailResult> GetCollectionArticle(int id)
        {
            return await CacheManager.GetCollectionArticleDetail(id);
        }

        public async Task<int> Insert(StoryDetailResult articleInfo, string image, bool multipic)
        {
            var db = new SQLiteConnectionWithLock(_sqLitePlatform, new SQLiteConnectionString(_dbPath, true));
            using (db.Lock())
            {
                await CacheManager.SetCollectionArticleDetail(articleInfo.Id, articleInfo);

                var collectionInfo = CollectionInfo.CreateFromStoryInfo(articleInfo, image, multipic);
               

                return db.InsertOrReplace(collectionInfo);
            }
        }


        public async Task<int> Delete(int id)
        {
            var db = new SQLiteConnectionWithLock(_sqLitePlatform, new SQLiteConnectionString(_dbPath, true));
            using (db.Lock())
            {
                await CacheManager.DeleteCollectionArticleDetail(id);
                return db.Delete<CollectionInfo>(id);
            }
        }

        public bool Any(int id)
        {
            var db = new SQLiteConnectionWithLock(_sqLitePlatform, new SQLiteConnectionString(_dbPath, true));
            using (db.Lock())
            {
                return db.Table<CollectionInfo>().Any(c => c.Id == id);
            }
        }

        public List<CollectionInfo> GetArticleList(int page, int pageCount)
        {
            var db = new SQLiteConnectionWithLock(_sqLitePlatform, new SQLiteConnectionString(_dbPath, true));
            using (db.Lock())
            {
                return db.Table<CollectionInfo>().OrderByDescending(i => i.CreateTime).Skip((page - 1)*pageCount).Take(pageCount).ToList();
            }
        }


        public int Clear()
        {
            var db = new SQLiteConnectionWithLock(_sqLitePlatform, new SQLiteConnectionString(_dbPath, true));
            using (db.Lock())
            {
                return db.DeleteAll<CollectionInfo>();
            }
        }
    }
}