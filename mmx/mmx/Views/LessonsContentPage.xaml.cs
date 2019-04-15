using mmx.Models;
using mmx.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mmx.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonsContentPage : TabbedPage
    {
        LessonsContentViewModel viewModel;
        public LessonsContentPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new LessonsContentViewModel();
        }

        public LessonsContentPage(string title)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new LessonsContentViewModel(title);
        }

        async void OnSentenceItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            await Navigation.PushAsync(new SpeechPage(item.Description));

            // 手动取消选择项目
            SentenceListView.SelectedItem = null;
        }

        async void OnWordItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            await Navigation.PushAsync(new SpeechPage(item.Description));

            // 手动取消选择项目
            WordListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.SentenceItems.Count == 0)
                viewModel.LoadSentenceItemsCommand.Execute(null);

            if (viewModel.WordItems.Count == 0)
                viewModel.LoadWordItemsCommand.Execute(null);
        }
    }
}