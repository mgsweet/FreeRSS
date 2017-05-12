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

            // 构建两个临时的实例来测试数据库的插入
            //var temp = new ArticleInfo()
            //{
            //    FeedId = "aaa",
            //    Title = "bbb",
            //    PubDate = "ccc",
            //    Source = "ddd",
            //    Description = "eee",
            //    Unread = true,
            //    Isstarred = false
            //};

            //var temp2 = new ArticleInfo()
            //{
            //    FeedId = "222",
            //    Title = "bbb",
            //    PubDate = "ccc",
            //    Source = "ddd",
            //    Description = "eee",
            //    Unread = true,
            //    Isstarred = false
            //};

            //List<ArticleInfo> a = new List<ArticleInfo>();
            //a.Clear();
            //a.Add(temp2);
            //a.Add(temp);

            // 这两个都可以自动增加的，美滋滋
            //var withoutWait = await _db.InsertAllAsync(a);

            Debug.WriteLine("Database Async Load successfully!");
        }

        // query为主的所有方法
        public async static Task<List<ArticleInfo>> QueryStarredOrUnreadArticlesAsync(string Tag)
        {
            if (Tag == "Unread")
            {
                var query = _db.Table<ArticleInfo>().Where(v => v.Unread.Equals(true));
                return await query.ToListAsync();
            }
            else if (Tag == "Isstarred")
            {
                var query = _db.Table<ArticleInfo>().Where(v => v.Isstarred.Equals(true));
                return await query.ToListAsync();
            }
            else
            {
                Debug.WriteLine("Wrong Tag");
                return new List<ArticleInfo>();
            }
        }

        public async static Task<List<ArticleInfo>> QueryAritclesOfOneFeed(string LinkString)
        {
            var feedsQuery = _db.Table<FeedInfo>().Where(v => v.Source.Equals(LinkString));
            var feed = await feedsQuery.FirstOrDefaultAsync();

            var articlesQuery = _db.Table<ArticleInfo>().Where(v => v.FeedId.Equals(feed.Id));
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
        //public async static Task InsertAFeed()
    }
}
