using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using freeRSS.Models;

namespace freeRSS.Services
{
    // 引入Schema里面的所有表的结构
    using freeRSS.Schema;

    public class SQLiteService
    {
        // 静态的sqlite链接实例
        private  static SQLiteAsyncConnection _db;

        // 静态方法创建全局连接实例，初始化建表
        public async static Task LoadDatabaseAsync()
        {
            _db = new SQLiteAsyncConnection("Storage.db");

            await _db.CreateTableAsync<FeedInfo>();
            await _db.CreateTableAsync<ArticleInfo>();

            Debug.WriteLine("Database Async Load successfully!");
        }

        // query为主的所有方法
        public async static Task<List<ArticleInfo>> QueryStarredOrUnreadArticlesAsync(string Tag)
        {
            if (Tag == "Unread")
            {
                var query = _db.Table<ArticleInfo>().Where(v => v.Unread.Equals(true)).OrderBy(v => v.Id);
                return await query.ToListAsync();
            }
            else if (Tag == "Isstarred")
            {
                var query = _db.Table<ArticleInfo>().Where(v => v.Isstarred.Equals(true)).OrderBy(v => v.Id);
                return await query.ToListAsync();
            }
            else
            {
                Debug.WriteLine("Wrong Tag");
                return new List<ArticleInfo>();
            }
        }


        // 这个函数其实应该可能可以舍掉了
        public async static Task<List<ArticleInfo>> QueryAritclesByFeedSource(string LinkString)
        {
            var feedsQuery = _db.Table<FeedInfo>().Where(v => v.Source.Equals(LinkString));
            var feed = await feedsQuery.FirstOrDefaultAsync();

            var articlesQuery = _db.Table<ArticleInfo>().Where(v => v.FeedId.Equals(feed.Id));
            return await articlesQuery.ToListAsync();
        }

        /// <summary>
        /// Try to get the old articles of a feedId in the database
        /// </summary>
        public async static Task<List<ArticleInfo>> QueryAritclesByFeedIdAsync(int feedId)
        {
            var articlesQuery = _db.Table<ArticleInfo>().Where(v => v.FeedId.Equals(feedId)).OrderBy(v => v.Id);
            return await articlesQuery.ToListAsync();
        }

        public async static Task<List<FeedInfo>> QueryAllFeeds()
        {
            var query = _db.Table<FeedInfo>();
            return await query.ToListAsync();
        }
        // Remove的方法
        public async static Task RemoveAFeed(string LinkString)
        {
            var target = new FeedInfo() { Source = LinkString };

            var res_count = await _db.DeleteAsync(target);
            if (res_count == 1)
            {
                Debug.WriteLine("Delete One Feed: {0}", LinkString);
            } else if (res_count == 0)
            {
                Debug.WriteLine("Feed No Delete!: {0}", LinkString);
            }
        }

        public async static Task RemoveAnArticle(string LinkString)
        {
            var target = new ArticleInfo() { Source = LinkString };

            var res_count = await _db.DeleteAsync(target);
            if (res_count == 1)
            {
                Debug.WriteLine("Delete One Article: {0}", LinkString);
            }
            else if (res_count == 0)
            {
                Debug.WriteLine("Article No Delete!: {0}", LinkString);
            }
        }

        // Insert Method
        public static async Task SaveFeedsInfoAsync(IEnumerable<FeedInfo> feeds)
        {
            foreach (var item in feeds)
            {
                await _db.InsertOrReplaceAsync(item);
            }
        }

        public static async Task SaveArticlesInfoAsync(IEnumerable<ArticleInfo> articles)
        {
            foreach (var item in articles)
            {
                await _db.InsertOrReplaceAsync(item);
            }
        }

        /// <summary>
        /// 如果f存在，则返回它的id；如果f不存在，则插入并返回它的插入后的id
        /// 同步方法
        /// 听说异步方法有bug。。。只能改同步work先
        /// </summary>
        public static int InsertOrReplaceFeed(FeedInfo f)
        {
            var conn = new SQLiteConnection("Storage.db");
            conn.Insert(f);
            conn.Close();
            return (int)f.Id;
        }
    }
}
