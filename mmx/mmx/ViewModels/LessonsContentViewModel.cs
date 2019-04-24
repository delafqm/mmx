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
    public class LessonsContentViewModel : BaseViewModel
    {
        public ObservableCollection<Lessons> SentenceItems { get; set; }
        public ObservableCollection<Lessons> WordItems { get; set; }
        public Command LoadSentenceItemsCommand { get; set; }
        public Command LoadWordItemsCommand { get; set; }

        public LessonsContentViewModel()
        {
            //Title = "课文内容";
            //SentenceItems = new ObservableCollection<Item>();
            //WordItems = new ObservableCollection<Item>();

            //LoadSentenceItemsCommand = new Command(async () => await ExecuteLoadSentenceItemsCommand());
            //LoadWordItemsCommand = new Command(async () => await ExecuteLoadWordItemsCommand());
        }

        public LessonsContentViewModel(Lessons item)
        {
            Title = item.Name;
            SentenceItems = new ObservableCollection<Lessons>();
            WordItems = new ObservableCollection<Lessons>();

            LoadSentenceItemsCommand = new Command(async () => await ExecuteLoadSentenceItemsCommand(item.Gid));
            LoadWordItemsCommand = new Command(async () => await ExecuteLoadWordItemsCommand(item.Gid));
        }

        /// <summary>
        /// 句子列表刷新执行的方法
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadSentenceItemsCommand(string sid)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                SentenceItems.Clear();
                var items = await sqliteData.GetSentenceItemsAsync(sid);//DataStore.GetSentenceItemsAsync(true);
                foreach (var item in items)
                {
                    SentenceItems.Add(item);
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

        /// <summary>
        /// 单词列表刷新执行的方法
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadWordItemsCommand(string sid)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                WordItems.Clear();
                var items = await sqliteData.GetWordItemsAsync(sid);//DataStore.GetWordItemsAsync(true);
                foreach (var item in items)
                {
                    WordItems.Add(item);
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
