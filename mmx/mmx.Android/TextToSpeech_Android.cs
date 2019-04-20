using Android.Speech.Tts;
using Xamarin.Forms;
using Java.Lang;
using mmx.Droid;
using System.Threading.Tasks;

[assembly: Dependency(typeof(TextToSpeech_Android))]
namespace mmx.Droid
{
    public class TextToSpeech_Android : Object, ITextToSpeech, TextToSpeech.IOnInitListener, TextToSpeech.IOnUtteranceCompletedListener
    {
        TextToSpeech speaker;
        Button _btn = null;

        string toSpeak;
        float _Speed = 1f;
        float _Pitch = 1f;
        string _msg;

        public void Speak(string text, float speed, float pitch ,Button btn,string msg)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _Speed = speed;
                _Pitch = pitch;
                _btn = btn;
                _msg = msg;
                toSpeak = text;
                if (speaker == null)
                {
                    speaker = new TextToSpeech(MainActivity.Instance, this);

                }
                else
                {
                    if (speaker.IsSpeaking)
                    {
                        speaker.Stop();
                    }
                    speaker.SetOnUtteranceProgressListener(new ttsUtteranceListener(_btn, _msg));
                    //speaker.SetOnUtteranceCompletedListener(this);
                    //speaker.SetLanguage(Java.Util.Locale.Us);//设置语言
                    speaker.SetPitch(_Pitch);//音高
                    speaker.SetSpeechRate(_Speed);//设置的语速
                    speaker.Speak(toSpeak, QueueMode.Flush, null, "UniqueID");
                    
                }

                //            ttsUtteranceListener abc = new ttsUtteranceListener()
                //            {
                //                 private override void onDone(String utteranceId) { }
                //};
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetOnUtteranceProgressListener(new ttsUtteranceListener(_btn, _msg));
                //speaker.SetOnUtteranceCompletedListener(this);
                speaker.SetLanguage(Java.Util.Locale.Us);//设置语言
                speaker.SetPitch(_Pitch);//音高
                speaker.SetSpeechRate(_Speed);//初始化的语速
                speaker.Speak(toSpeak, QueueMode.Flush, null, "UniqueID");
            }
        }

        public void OnUtteranceCompleted(string utteranceId)
        {
            var activity = MainActivity.Instance;
            activity.RunOnUiThread(() => {
                //Android.Widget.Toast.MakeText(MainActivity.Instance, "播放完成", Android.Widget.ToastLength.Long).Show();
                _btn.Text = "3";
            });
            //_abc = "播放完成";
            //Android.Widget.Toast.MakeText(MainActivity.Instance, "播放完成", Android.Widget.ToastLength.Long).Show();
            //if (btnSpeech != null)
            //{
            //    btnSpeech.Text = "3";
            //}
            //btnSpeech.Text = _name;
            //throw new System.NotImplementedException();
        }

        #endregion
    }

    public class ttsUtteranceListener : UtteranceProgressListener
    {
        Button _btn;
        string _text;

        public ttsUtteranceListener()
        { }

        public ttsUtteranceListener(Button btn,string text)
        {
            _btn = btn;
            _text = text;
        }

        public override void OnDone(string utteranceId)
        {
            var activity = MainActivity.Instance;
            activity.RunOnUiThread(() => {
                //Android.Widget.Toast.MakeText(MainActivity.Instance, "播放完成", Android.Widget.ToastLength.Long).Show();
                _btn.Text = _text;
                _btn.IsEnabled = true;
            });
        }

        public override void OnError(string utteranceId)
        {
            //throw new System.NotImplementedException();
        }

        public override void OnStart(string utteranceId)
        {
            //throw new System.NotImplementedException();
        }
    }
}