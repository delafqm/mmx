﻿using Android.Speech.Tts;
using Xamarin.Forms;
using Java.Lang;
using mmx.Droid;
using System.Threading.Tasks;

[assembly: Dependency(typeof(TextToSpeech_Android))]
namespace mmx.Droid
{
    public class TextToSpeech_Android : Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        Button btnSpeech;
        string _name;

        string toSpeak;
        float _Speed = 1f;
        float _Pitch = 1f;

        public void Speak(string text, float speed, float pitch, Button btn, string name)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _Speed = speed;
                _Pitch = pitch;
                _name = name;
                btnSpeech = btn;
                toSpeak = text;

                if (speaker == null)
                {
                    speaker = new TextToSpeech(MainActivity.Instance, this);

                }
                else
                {
                    speaker.SetOnUtteranceProgressListener(new ttsUtteranceListener(ref btnSpeech, _name));
                    speaker.SetLanguage(Java.Util.Locale.Us);//设置语言
                    speaker.SetPitch(_Pitch);//音高
                    speaker.SetSpeechRate(_Speed);//设置的语速
                    speaker.Speak(toSpeak, QueueMode.Flush, null, "UniqueID");
                }

                ttsUtteranceListener abc = new ttsUtteranceListener()
                {
                     private override void onDone(String utteranceId) { }
    };
}
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetOnUtteranceProgressListener(new ttsUtteranceListener(ref btnSpeech, _name));
                speaker.SetLanguage(Java.Util.Locale.Us);//设置语言
                speaker.SetPitch(_Pitch);//音高
                speaker.SetSpeechRate(_Speed);//初始化的语速
                speaker.Speak(toSpeak, QueueMode.Flush, null, "UniqueID");
            }

        }

        #endregion
    }

    public class ttsUtteranceListener : UtteranceProgressListener
    {
        Button _btn;
        string _name;

        public ttsUtteranceListener()
        { }

        public ttsUtteranceListener(ref Button btn, string name)
        {
            _btn = btn;
            _name = name;
        }

        public override void OnDone(string utteranceId)
        {
            _btn.Text = _name;
            _btn.IsEnabled = true;
            //throw new System.NotImplementedException();
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