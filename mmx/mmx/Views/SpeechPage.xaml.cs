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


        public SpeechPage ()
		{
			InitializeComponent ();
		}

        public SpeechPage(string Text)
        {
            InitializeComponent();

            InputText.Text = Text;
            InputText.IsEnabled = false;
            //BindingContext
        }

        void OnSpeakClicked(object sender, EventArgs e)
        {
            float _speed = 1f;
            float _pitch = 1f;
            bool isError = false;
            try
            {
                _speed = Convert.ToSingle(Speed.Text.Trim());
                _pitch = Convert.ToSingle(Pitch.Text.Trim());
            }
            catch (Exception ex)
            {
                lblStatus1.Text = "数据转换出错";
                isError = true;
            }
            //var todoItem = (TodoItem)BindingContext;
            if (!isError)
                DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), _speed, _pitch);
        }

        void OnSlowSpeakClicked(object sender, EventArgs e)
        {
            //var todoItem = (TodoItem)BindingContext;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 0.5f, 1f);
        }

        void OnSuperSlowSpeakClicked(object sender, EventArgs e)
        {
            //var todoItem = (TodoItem)BindingContext;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 0.1f, 1f);
        }

        void OnRecordPressed(object sender, EventArgs e)
        {
            DependencyService.Get<IAudioRecorder>().Start(filepath);

            lblStatus1.Text = "音频录制中";
        }

        async void OnRecordReleased(object sender, EventArgs e)
        {
            DependencyService.Get<IAudioRecorder>().Stop();

            lblStatus1.Text = "正在识别中";
            //使用百度API进行语音识别
            //OutputText.Text = await ToTextByBaidu();
            var result = await ToTextByBaidu();
            OutputText.Text = result;
            lblStatus1.Text = "";
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
            if (File.Exists(filepath))
            {
                DependencyService.Get<IAudioRecorder>().Play(filepath);
            }
            else
            {
                lblStatus1.Text = "无录音";
            }
        }
    }
}