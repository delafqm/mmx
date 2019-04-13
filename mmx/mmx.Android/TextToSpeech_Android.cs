using Android.Speech.Tts;
using Xamarin.Forms;
using Java.Lang;
using mmx.Droid;

[assembly: Dependency(typeof(TextToSpeech_Android))]
namespace mmx.Droid
{
    public class TextToSpeech_Android : Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text, float speed)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                toSpeak = text;
                if (speaker == null)
                {
                    speaker = new TextToSpeech(MainActivity.Instance, this);
                    speaker.SetLanguage(Java.Util.Locale.Us);
                    speaker.SetSpeechRate(speed);//初始化的语速
                }
                else
                {
                    speaker.SetSpeechRate(speed);//设置的语速
                    speaker.Speak(toSpeak, QueueMode.Flush, null, null);
                }
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
        #endregion
    }
}