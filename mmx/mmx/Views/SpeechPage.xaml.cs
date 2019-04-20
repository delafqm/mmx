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
            float _speek = 1f;
            if (SelectSpeek.SelectedIndex != -1)
            {
                _speek = mySpeek[SelectSpeek.Items[SelectSpeek.SelectedIndex]];
            }


            BtnSpeak.Text = "▶";
            BtnSpeak.IsEnabled = false;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), _speek, 1f, BtnSpeak, "🔊");
            //DependencyService.Get<ITextToSpeech>().abc
        }

        void OnSlowSpeakClicked(object sender, EventArgs e)
        {
            //BtnSlowSpeak.Text = "播放中";
            //DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 0.5f, 1f, BtnSpeak);

            //测试用，调用百度语音合成API
            //SpeechResult result = await mmx.Speech.Tts(InputText.Text.Trim(), _spd, _pit);
        }

        void OnSuperSlowSpeakClicked(object sender, EventArgs e)
        {
            //BtnSuperSlowSpeak.Text= "播放中";
            //DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 0.1f, 1f, BtnSpeak);

            //测试用，调用迅飞语音合成API
            //string result = mmx.Speech.Headers(InputText.Text.Trim(), filexunfei, _spd.ToString());
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

        static async Task<string> ToTextByBaidu()
        {
            string resultmsg = "";
            if (File.Exists(filepath))
            {
                var APP_ID = "14965195";
                var API_KEY = "R2qXXgwr9xKtge3kxU5U7up2";
                var SECRET_KEY = "Gnm2KhHcgZEDDLwy0Qtl66y4fFc8FmTj";
                var client = new Baidu.Aip.Speech.Asr(APP_ID, API_KEY, SECRET_KEY);
                client.Timeout = 60000;  // 修改超时时间

                //读取文件
                //string rootPath = Directory.GetCurrentDirectory();
                var data = File.ReadAllBytes(filepath);

                //识别语种，英文1737;
                Dictionary<string, object> op = new Dictionary<string, object>();
                op["dev_pid"] = 1737;

                //client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms

                var res = Task.Run(() =>
                 {
                     var result = client.Recognize(data, "amr", 16000, op);

                     MResult mResult = JsonConvert.DeserializeObject<MResult>(result.ToString());

                     if (mResult.err_no == 0)
                     {
                         return mResult.result[0].ToString();
                     }
                     else
                     {
                         return "语音错误：" + mResult.err_no.ToString();
                     }
                 });
                resultmsg = await res;
            }
            else
            {
                resultmsg = "语音错误：无语音";
            }

            return resultmsg;
                
        }

        void OnPlayClicked(object sender, EventArgs e)
        {
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