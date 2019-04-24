using mmx.Models;
using mmx.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mmx.ViewModels
{
    public class LessonsViewModel : BaseViewModel
    {
        public ObservableCollection<Lessons> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public LessonsViewModel()
        {
            Title = "课文";
            Items = new ObservableCollection<Lessons>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(""));
        }

        public LessonsViewModel(Lessons lessons)
        {
            Title = lessons.Name;
            Items = new ObservableCollection<Lessons>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(lessons.Gid));
        }

        async Task ExecuteLoadItemsCommand(string sid)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await sqliteData.GetLessonsItemsAsync(sid);//DataStore.GetLessonsItemsAsync(true);
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
    }
}
