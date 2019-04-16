using mmx.Models;
using mmx.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mmx.Data
{
    public class WebData
    {
        public async Task UpDataAsync(string data)
        {
            SqliteDataStore datastore = new SqliteDataStore("");
            List<Words> wrods = JsonConvert.DeserializeObject<List<Words>>(data);
            var phoneWrods= await datastore.GetWordItemsAsync();
            bool iss = false;
            foreach (var word in wrods)
            {
                foreach (var pwrod in phoneWrods)
                {
                    if (word.Gid == pwrod.Gid)
                    {
                        word.Id = 0;//设置ID为0，表示新增，非零为修改
                    }
                }
                await datastore.AddItemAsync(word);//修改数据，看ID号，零为新增，非零为修改
            }
        }
    }
}
