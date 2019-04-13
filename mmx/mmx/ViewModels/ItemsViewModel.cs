using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using mmx.Models;
using mmx.Views;
using System.Collections.Generic;

namespace mmx.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "年级";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = GetItems();//await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public List<Item> GetItems()
        {
            List<Item> items;
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "三年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "四年级下册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级上册", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "五年级下册", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }

            return items;
        }
    }
}