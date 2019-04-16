using mmx.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mmx.Services
{
    class SqliteDataStore : IDataStore<Words>
    {
        readonly SQLiteAsyncConnection database;

        public SqliteDataStore(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Words>().Wait();
        }

        public async Task<bool> AddItemAsync(Words item)
        {
            int res = 0;
            if (item.Id != 0)
            {
                res = await database.UpdateAsync(item);
            }
            else
            {
                res = await database.InsertAsync(item);
            }

            return res > 0 ? true : false;
        }

        public async Task<bool> DeleteItemAsync(Words item)
        {
            int res = 0;
            res = await database.DeleteAsync(item);
            return res > 0 ? true : false;
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Words>> GetGradeItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<Words> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Words> GetItemAsync(int id)
        {
            return database.Table<Words>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Words>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Words>> GetLessonsItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Words>> GetSentenceItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Words>> GetWordItemsAsync(bool forceRefresh = false)
        {
            return await database.QueryAsync<Words>("SELECT * FROM [Words]");
        }

        public Task<bool> UpdateItemAsync(Words item)
        {
            throw new NotImplementedException();
        }
    }
}
