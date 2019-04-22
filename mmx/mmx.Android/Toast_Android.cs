using Android.Widget;
using mmx.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toast_Android))]
namespace mmx.Droid
{
    public class Toast_Android : IToast
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(MainActivity.Instance, message, ToastLength.Long).Show();
        }
        public void ShortAlert(string message)
        {
            Toast.MakeText(MainActivity.Instance, message, ToastLength.Short).Show();
        }
    }
}