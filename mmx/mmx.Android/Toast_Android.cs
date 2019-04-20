using Android.Widget;
using mmx.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toast_Android))]
namespace mmx.Droid
{
    public class Toast_Android
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}