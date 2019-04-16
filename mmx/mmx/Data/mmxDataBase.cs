using mmx.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mmx.Data
{

    public class mmxDataBase
    {
        readonly SQLiteAsyncConnection database;

        public mmxDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Words>().Wait();
        }

        public Task<List<Words>> GetItemsAsync()
        {
            return database.Table<Words>().ToListAsync();
        }

        public Task<List<Words>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Words>("SELECT * FROM [Words]");
        }

        public Task<Words> GetItemAsync(int id)
        {
            return database.Table<Words>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Words item)
        {
            if (0 != item.Id)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Words item)
        {
            return database.DeleteAsync(item);
        }
    }
}
