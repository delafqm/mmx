using mmx.Models;
using mmx.Services;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public ObservableCollection<Lessons> Items { get; set; }

        SqliteDataStore sqlitedata;

        //测试先用模拟数据
        //public IDataStore<Item> DataStore = new MockDataStore(); 
        //使用网络数据
        //public IDataStore<Item> DataStore = new AzureDataStore();

        HttpClient client;

        public Update()
        {
            InitializeComponent();

            client = new HttpClient();

            Items = new ObservableCollection<Lessons>();

            ItemsListView.ItemsSource = Items;

            sqlitedata = new SqliteDataStore();
        }

        async Task UpdateByWeb()
        {
            show1.IsVisible = true;
            show2.IsVisible = false;
            //ShowRun.IsRunning = true;
            //await Task.Run(() => {
            //    Thread.Sleep(3000);
            //});
            //从数据读取
            var items1 = await sqlitedata.GetItemsAsync();
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
            bool isupdate = true;//是否需要远程更新标记

            
            Settings localSettings = null;//本地状态
            Settings remoteSettings = null;//远程状态

            #region 查看远程数据库更新状态
            //先获取本机数据
            localSettings = await sqlitedata.GetSettingsAsync();
            try
            {
                //远程数据库读取数据
                var setjson = await client.GetStringAsync($"http://47.99.36.29:8080/api/GetLastUpdate");
                remoteSettings = await Task.Run(() => JsonConvert.DeserializeObject<Settings>(setjson));

                if (localSettings != null && remoteSettings != null)
                    isupdate = localSettings.Update != remoteSettings.Update ? true : false;

                if (isupdate)
                {
                    DependencyService.Get<IToast>().LongAlert("获取远程状态信息:需要更新数据");
                }
                else
                {
                    DependencyService.Get<IToast>().LongAlert("获取远程状态信息：不需要更新数据");
                }
            }
            catch (Exception ex)
            { }
            

            #endregion

            if (isupdate)
            {
                #region 获取远程数据
                //远程数据临时存放实体
                IEnumerable<Lessons> lessons = null;
                try
                {
                    var json = await client.GetStringAsync($"http://47.99.36.29:8080/api/GetLessonsListAll");
                    if (json != null)
                    {
                        lessons = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Lessons>>(json));
                    }
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IToast>().LongAlert("访问网络出错");
                }
                //从模拟数据读取
                //var items = await DataStore.GetLessonsItemsAsync(true);
                #endregion

                #region 写入本机数据库

                if (lessons != null)
                {
                    DependencyService.Get<IToast>().LongAlert("删除原有数据");
                    int itemCount = 0;
                    try
                    {

                        //删除表所有数据
                        await sqlitedata.DeleteLessonsAllAsync();

                        foreach (var item in lessons)
                        {
                            item.Id = 0;//设置ID空为新建
                            await sqlitedata.SaveItemAsync(item);//写入数据库
                            itemCount++;
                        }

                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IToast>().LongAlert("写入本机数据库出错");
                    }

                    DependencyService.Get<IToast>().LongAlert("完成添加数据，已添加" + itemCount.ToString() + "条。");
                }

                #endregion

                #region 更新本机数据库更新状态
                if (remoteSettings != null)
                {
                    if(localSettings==null)
                    {
                        localSettings = new Settings();
                        localSettings.Id = 0;
                        localSettings.Name = "lastupdate";
                    }
                    //跟远程更新状态时间同步
                    localSettings.Update = remoteSettings.Update;
                    //保存本地数据
                    await sqlitedata.SaveSettingsOneAsync(localSettings);

                    DependencyService.Get<IToast>().LongAlert("更新本地状态");
                }
                //localSettings.Update = DateTime.Now;
                
                #endregion
            }

            //设置控件显示状态
            show1.IsVisible = false;
            show2.IsVisible = true;
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