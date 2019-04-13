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
        float _speed;

        public void Speak(string text, float speed)
        {
            _speed = speed;
            if (!string.IsNullOrWhiteSpace(text))
            {
                toSpeak = text;
                if (speaker == null)
                {
                    speaker = new TextToSpeech(MainActivity.Instance, this);
                }
                else
                {
                    speaker.SetLanguage(Java.Util.Locale.Us);
                    speaker.SetSpeechRate(_speed);//设置的语速
                    speaker.Speak(toSpeak, QueueMode.Flush, null, null);
                }
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetLanguage(Java.Util.Locale.Us);
                speaker.SetSpeechRate(_speed);//初始化的语速
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
        #endregion
    }
}