using mmx.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace mmx.ViewModels
{
    class SpeechViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }

    }
}
