﻿using mmx.Models;
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

        public SpeechPage()
        {
            InitializeComponent();
        }

        public SpeechPage(string text)
        {
            InitializeComponent();

            //BindingContext
        }

        void OnSpeakClicked(object sender, EventArgs e)
        {
            //var todoItem = (TodoItem)BindingContext;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 1f);
        }

        void OnSlowSpeakClicked(object sender, EventArgs e)
        {
            //var todoItem = (TodoItem)BindingContext;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 0.5f);
        }

        void OnSuperSlowSpeakClicked(object sender, EventArgs e)
        {
            //var todoItem = (TodoItem)BindingContext;
            DependencyService.Get<ITextToSpeech>().Speak(InputText.Text.Trim(), 0.1f);
        }

        void OnStartClicked(object sender, EventArgs e)
        {
            if (File.Exists(filepath))
                DependencyService.Get<IAudioRecorder>().Play(filepath);
        }

        void OnPressed(object sender, EventArgs e)
        {
            DependencyService.Get<IAudioRecorder>().Start(filepath);
        }

        void OnReleased(object sender, EventArgs e)
        {
            DependencyService.Get<IAudioRecorder>().Stop();
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
                var result = client.Recognize(data, "amr", 16000, op);

                MResult mResult = JsonConvert.DeserializeObject<MResult>(result.ToString());

                if (mResult.err_no == 0)
                {
                    OutputText.Text = mResult.result[0].ToString();
                }
                else
                {
                    OutputText.Text = mResult.err_no.ToString();
                }
            }
            else
            {
                OutputText.Text = "无语音";
            }
        }
    }
}
