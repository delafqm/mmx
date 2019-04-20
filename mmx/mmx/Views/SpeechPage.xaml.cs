using mmx.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mmx.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SpeechPage : ContentPage
	{
        static string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testAudio.amr");
        //static string filemp3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testAudio.mp3");
        //static string filexunfei = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testAudioxunfei.mp3");

        Dictionary<string, float> mySpeek = new Dictionary<string, float>
        {
            {"正常速度朗读",1f },
            {"一半速度朗读",0.5f },
            {"1/10速度朗读",0.1f }
        };

        public SpeechPage ()
		{
			InitializeComponent ();
		}

        public SpeechPage(string Text)
        {
            InitializeComponent();

            InputText.Text = Text;
            BtnSpeak.Text = "🔊";

            
            foreach (var speek in mySpeek.Keys)
            {
                SelectSpeek.Items.Add(speek);
            }

            //InputText.IsEnabled = false;
            //BindingContext
        }

        void SelectSpeekChanged(object sender, EventArgs e)
        {
        }

        void OnSpeakClicked(object sender, EventArgs e)
        {
            if(btnPlay.IsEnabled==false)
            {
                //DependencyService.Get<IToast>().LongAlert("正在语音识别的朗读");
                return;
            }
            float _speek = 1f;
            if (SelectSpeek.SelectedIndex != -1)
            {
                _speek = mySpeek[SelectSpeek.Items[SelectSpeek.SelectedIndex]];
            }


            BtnSpeak.Text = "▶";
            BtnSpeak.IsEnabled = false;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), _speek, 1f, BtnSpeak, "🔊");
        }

        void OnRecordPressed(object sender, EventArgs e)
        {
            btnRecord.Text = "正录制中";
            DependencyService.Get<IAudioRecorder>().Start(filepath);
        }

        async void OnRecordReleased(object sender, EventArgs e)
        {
            DependencyService.Get<IAudioRecorder>().Stop();

            btnRecord.Text = "正识别中";
            //使用百度API进行语音识别
            //OutputText.Text = await ToTextByBaidu();
            SpeechResult result = await mmx.Speech.Asr(filepath);
            OutputText.Text = result.text;
            btnRecord.Text = "录音识别";
        }

        void OnPlayClicked(object sender, EventArgs e)
        {
            if(BtnSpeak.IsEnabled==false)
            {
                //DependencyService.Get<IToast>().LongAlert("正在语音合成的朗读");
                return;
            }
            playmp3(filepath);
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="fp">声音文件地址</param>
        void playmp3(string fp)
        {
            if (File.Exists(fp))
            {
                btnPlay.Text = "正播放中";
                btnPlay.IsEnabled = false;
                DependencyService.Get<IAudioRecorder>().Play(fp, btnPlay, "播放录音");
            }
            else
            {
                //弹出提示
                DependencyService.Get<IToast>().LongAlert("无语音文件");
            }
        }
    }
}