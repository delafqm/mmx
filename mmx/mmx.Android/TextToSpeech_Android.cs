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
        float _Speed = 1f;
        float _Pitch = 1f;

        public void Speak(string text, float speed, float pitch)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _Speed = speed;
                _Pitch = pitch;
                toSpeak = text;
                
                if (speaker == null)
                {
                    speaker = new TextToSpeech(MainActivity.Instance, this);
                }
                else
                {
                    speaker.SetLanguage(Java.Util.Locale.Us);
                    speaker.SetPitch(_Pitch);//音高
                    speaker.SetSpeechRate(_Speed);//设置的语速
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
                speaker.SetPitch(_Pitch);//音高
                speaker.SetSpeechRate(_Speed);//初始化的语速
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
        #endregion
    }
}