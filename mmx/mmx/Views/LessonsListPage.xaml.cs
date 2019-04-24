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
	public partial class LessonsListPage : ContentPage
	{
        LessonsViewModel viewModel;

        public LessonsListPage ()
		{
			InitializeComponent ();

            BindingContext = this.viewModel = new LessonsViewModel();
        }

        public LessonsListPage(Lessons item)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new LessonsViewModel(item);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Lessons;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            await Navigation.PushAsync(new LessonsContentPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}