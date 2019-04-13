﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using mmx.Droid;
using Android.Media;
using System.IO;

[assembly: Dependency(typeof(AudioRecorder_Android))]
namespace mmx.Droid
{
    public class AudioRecorder_Android : Object, IAudioRecorder
    {
        MediaRecorder recorder = null;

        static string filePath = Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryMusic + "/testAudio.amr";

        public void Start()
        {
            try
            {
                //Java.IO.File sdDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic);
                //filePath = sdDir + "/" + "testAudio.mp3";
                if (File.Exists(filePath))
                    File.Delete(filePath);

                //Java.IO.File myFile = new Java.IO.File(filePath);
                //myFile.CreateNewFile();

                if (recorder == null)
                    recorder = new MediaRecorder(); // Initial state.
                else
                    recorder.Reset(); //录音重置

                recorder.SetAudioSource(AudioSource.Mic);
                recorder.SetOutputFormat(OutputFormat.AmrWb);//设置输出格式
                recorder.SetAudioEncoder(AudioEncoder.AmrWb); //设置编码器
                recorder.SetAudioSamplingRate(16000);//设置采样率
                recorder.SetOutputFile(filePath); //保存路径

                recorder.Prepare(); // 录音准备-固定顺序
                recorder.Start(); // 录音开始-固定顺序

            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.Message, ToastLength.Long).Show();
            }
        }

        public string Stop()
        {
            if (recorder != null)
            {
                try
                {
                    recorder.Stop();
                    recorder.Reset();
                    recorder.Release();//录音资源释放
                    recorder = null;
                    return filePath;
                }
                catch (Java.Lang.IllegalStateException ex)
                {
                    recorder.Reset();
                    recorder.Release();
                    recorder = null;
                    return null;
                }
            }
            return null;
        }
    }
}