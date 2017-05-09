using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace freeRSS.Services
{
    public class SQLiteService
    {
        // 静态的sqlite链接实例
        public static SQLiteConnection db;

        // 定义表一feeds的schema
        public class feeds
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [MaxLength(40), NotNull]
            public string Name { get; set; }

            [NotNull]
            public string Source { get; set; }

            [NotNull]
            public string IconSrc { get; set; }

            [NotNull]
            public string LastBuildedTime { get; set; }

            [MaxLength(96)]
            public string Description { get; set; }
        }

        // 定义表二articles的schema
        public class articles
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [NotNull]
            public int FeedId { get; set; }

            [NotNull]
            public string Title { get; set; }

            public string PubDate { get; set; }
            [NotNull]
            public string Source { get; set; }
            [NotNull]
            public string Description { get; set; }

            [NotNull]
            public bool Unread { get; set; }
            [NotNull]
            public bool Isstarred { get; set; }
        }


        // 静态方法创建全局连接实例，初始化建表
        public static void LoadDatabase()
        {
            db = new SQLiteConnection("Storage.db");

            db.CreateTable<feeds>();
            db.CreateTable<articles>();
        }
    }
}
