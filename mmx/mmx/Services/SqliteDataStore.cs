using mmx.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace mmx.Services
{
    public class SqliteDataStore
    {
        readonly SQLiteAsyncConnection database;

        public SqliteDataStore()
        {
            //初始化本地数据库连接
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mmxSQLite.db3"));
            //创建表
            //比较实体与表结构，不相同时自动进行迁移。
            database.CreateTableAsync<Lessons>().Wait();
            database.CreateTableAsync<Settings>().Wait();
        }

        public async Task<IEnumerable<Lessons>> GetGradeItemsAsync()
        {
            return await database.QueryAsync<Lessons>("select * from [Lessons] where types='G'");
        }

        public async Task<IEnumerable<Lessons>> GetLessonsItemsAsync(string grade)
        {
            return await database.QueryAsync<Lessons>("select * from [Lessons] where types='L' and sid='" + grade + "'");
        }

        public async Task<IEnumerable<Lessons>> GetSentenceItemsAsync(string lessons)
        {
            return await database.QueryAsync<Lessons>("select * from [Lessons] where types='S' and sid='" + lessons + "'");
        }

        public async Task<IEnumerable<Lessons>> GetWordItemsAsync(string lessons)
        {
            return await database.QueryAsync<Lessons>("select * from [Lessons] where types='W' and sid='" + lessons + "'");
        }

        public async Task<Settings> GetSettingsAsync()
        {
            return await database.FindWithQueryAsync<Settings>("select * from [Settings] where Name='lastupdate'");
        }

        public async Task<int> SaveSettingsOneAsync(Settings settings)
        {
            if (settings.Id != 0)
            {
                return await database.UpdateAsync(settings);
            }
            else
                return await database.InsertAsync(settings);
        }

        public async Task<List<Lessons>> GetItemsAsync()
        {
            return await database.Table<Lessons>().ToListAsync();
        }

        public async Task<List<Lessons>> GetItemsNotDoneAsync()
        {
            return await database.QueryAsync<Lessons>("SELECT * FROM [Lessons]");
        }

        public async Task<Lessons> GetItemAsync(int id)
        {
            return await database.Table<Lessons>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Lessons item)
        {
            if (item.Id != 0)
            {
                return await database.UpdateAsync(item);
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(Lessons item)
        {
            return await database.DeleteAsync(item);
        }

        public async Task<int> DeleteLessonsAllAsync()
        {
            return await database.DeleteAllAsync<Lessons>();
        }

    }
}
