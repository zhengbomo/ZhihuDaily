/* ==============================================================================
 * 功能描述：CacheManager  
 * 创 建 者：贤凯
 * 创建日期：2/6/2015 7:33:43 PM
 * ==============================================================================*/

using System.Collections.Generic;
using System.Threading.Tasks;
using ZhihuDaily.Domain.Models;
using Shagu.Utils.Utils;

namespace ZhihuDaily.Domain.Service
{
    public class CacheManager : CacheManagerBase
    {
        //缓存收藏列表
        public static async Task<FavoriteResult> GetFaviroteResult()
        {
            var path = string.Format("cachedata\\favorite\\favorite.db");
            return await ReadFile<FavoriteResult>(path, null);
        }
        
        public static async Task SetFaviroteResult(FavoriteResult articleDetail)
        {
            var path = string.Format("cachedata\\favorite\\favorite.db");
            await SaveFile(path, articleDetail);
        }
        
         
        //缓存收藏文章
        public static async Task<StoryDetailResult> GetCollectionArticleDetail(int articleId)
        {
            var path = string.Format("collection\\article\\{0}.db", articleId);
            return await ReadFile<StoryDetailResult>(path, null);
        }

        public static async Task SetCollectionArticleDetail(int articleId, StoryDetailResult articleDetail)
        {
            var path = string.Format("collection\\article\\{0}.db", articleId);
            await SaveFile(path, articleDetail);
        }

        public static async Task DeleteCollectionArticleDetail(int articleId)
        {
            var path = string.Format("collection\\article\\{0}.db", articleId);
            await Delete(path);
        }
        
        //缓存分类
        public static async Task<CategoryResult> GetCategory()
        {
            var path = string.Format("cachedata\\category.db");
            return await ReadFile<CategoryResult>(path, null);
        }

        public static async Task SetCategory(CategoryResult articleDetail)
        {
            var path = string.Format("cachedata\\category.db");
            await SaveFile(path, articleDetail);
        }

        //缓存推荐
        public static async Task<RecommendStoryResult> GetRecommendStory()
        {
            var path = string.Format("cachedata\\recommendStory.db");
            return await ReadFile<RecommendStoryResult>(path, null);
        }

        public static async Task SetRecommendStory(RecommendStoryResult articleDetail)
        {
            var path = string.Format("cachedata\\recommendStory.db");
            await SaveFile(path, articleDetail);
        }

        //缓存分类文章
        public static async Task<CategoryDetailResult> GetCategoryDetail(int categoryId)
        {
            var path = string.Format("cachedata\\category_{0}.db", categoryId);
            return await ReadFile<CategoryDetailResult>(path, null);
        }

        public static async Task SetCategoryDetail(int categoryId, CategoryDetailResult articleDetail)
        {
            var path = string.Format("cachedata\\category_{0}.db", categoryId);
            await SaveFile(path, articleDetail);
        }

        //缓存主列表
        public static async Task<StoryListResult> GetHomeStory()
        {
            var path = string.Format("cachedata\\homeStory.db");
            return await ReadFile<StoryListResult>(path, null);
        }

        public static async Task SetHomeStory(StoryListResult articleDetail)
        {
            var path = string.Format("cachedata\\homeStory.db");
            await SaveFile(path, articleDetail);
        }

        //缓存文章
        public static async Task<StoryDetailResult> GetStoryDetail(int storyId)
        {
            var path = string.Format("cachedata\\homeStory.db");
            return await ReadFile<StoryDetailResult>(path, null);
        }

        public static async Task SetStoryDetail(int storyId, StoryDetailResult articleDetail)
        {
            var path = string.Format("cachedata\\homeStory.db");
            await SaveFile(path, articleDetail);
        }
    }
}
