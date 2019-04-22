using mmx.Models;
using mmx.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mmx.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Update : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; }

        //本地数据库连接
        readonly SQLiteAsyncConnection database;

        //测试先用模拟数据
        //public IDataStore<Item> DataStore = new MockDataStore(); 
        //使用网络数据
        public IDataStore<Item> DataStore = new AzureDataStore();
        public Update()
        {
            InitializeComponent();

            Items = new ObservableCollection<Item>();

            ItemsListView.ItemsSource = Items;

            //初始化本地数据库连接
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mmxSQLite.db3"));
            //创建表
            //比较实体与表结构，不相同时自动进行迁移。
            database.CreateTableAsync<Item>().Wait();
            
        }

        async Task UpdateByWeb()
        {
            show1.IsVisible = true;
            show2.IsVisible = false;
            //ShowRun.IsRunning = true;
            await Task.Run(() => {
                Thread.Sleep(3000);
            });
            //从数据读取
            var items1 = await GetItemsAsync();
            DependencyService.Get<IToast>().LongAlert("从数据获取：" + items1.Count().ToString() + "条。");
            Items.Clear();
            foreach (var item in items1)
            {
                Items.Add(item);
            }
            ItemsListView.ItemsSource = Items;
            //ShowRun.IsRunning = false;
            show1.IsVisible = false;
            show2.IsVisible = true;
        }

        async Task AddDate()
        {
            show1.IsVisible = true;
            show2.IsVisible = false;
            DependencyService.Get<IToast>().LongAlert("点击了添加数据按钮");
            //从模拟数据读取
            var items = await DataStore.GetLessonsItemsAsync(true);
            DependencyService.Get<IToast>().LongAlert("删除原有数据");
            //删除表所有数据
            await database.DeleteAllAsync<Item>();
            int itemCount = 0;
            foreach (var item in items)
            {
                item.Id = null;//设置ID空为新建
                await SaveItemAsync(item);//写入数据库
                itemCount++;
            }

            DependencyService.Get<IToast>().LongAlert("完成添加数据，已添加" + itemCount.ToString() + "条。");
            show1.IsVisible = false;
            show2.IsVisible = true;
        }
        

        public Task<List<Item>> GetItemsAsync()
        {
            return database.Table<Item>().ToListAsync();
        }

        public Task<List<Item>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Item>("SELECT * FROM [Item]");
        }

        public Task<Item> GetItemAsync(string id)
        {
            return database.Table<Item>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Item item)
        {
            if (item.Id != null)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                item.Id = Guid.NewGuid().ToString();
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Item item)
        {
            return database.DeleteAsync(item);
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await AddDate();
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            await UpdateByWeb();
        }
    }
}