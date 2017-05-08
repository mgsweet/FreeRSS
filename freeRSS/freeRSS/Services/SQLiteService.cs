using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace freeRSS.Services
{
    public class SQLiteService
    {
        public static SQLiteConnection conn;

        public static void LoadDatabase()
        {
            conn = new SQLiteConnection("storage.db");

            string sql = @"CREATE TABLE IF NOT EXISTS
                                todo_items (
                                    id      VARCHAR( 36 ) PRIMARY KEY NOT NULL,
                                    title   VARCHAR( 100 ) NOT NULL,
                                    description    VARCHAR(150) NOT NULL,
                                    completed     INT NOT NULL,
                                    datetime        VARCHAR(40) NOT NULL,
                                    image_uri       VARCHAR(200)
                            );";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }
        }
    }
}
