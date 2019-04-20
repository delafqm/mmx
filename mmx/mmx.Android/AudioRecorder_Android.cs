using System;
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
    public class AudioRecorder_Android : Java.Lang.Object, IAudioRecorder, MediaPlayer.IOnCompletionListener
    {
        MediaRecorder recorder = null;
        MediaPlayer mMediaPlayer = null;
        Xamarin.Forms.Button _btn;
        string _name;

        //public IntPtr Handle => throw new NotImplementedException();

        //static string filePath = Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryMusic + "/testAudio.amr";

        public void Start(string filePath)
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

        public void Stop()
        {
            if (recorder != null)
            {
                try
                {
                    recorder.Stop();
                    recorder.Reset();
                    recorder.Release();//录音资源释放
                    recorder = null;
                }
                catch (Java.Lang.IllegalStateException ex)
                {
                    recorder.Reset();
                    recorder.Release();
                    recorder = null;
                }
            }
        }

        public void Play(string filepath, Xamarin.Forms.Button btn, string name)
        {
            _btn = btn;
            _name = name;

            try
            {
                if (mMediaPlayer == null)
                {
                    mMediaPlayer = new MediaPlayer();
                }
                else
                {
                    mMediaPlayer.Reset();
                }

                mMediaPlayer.SetDataSource(filepath);//设置播放源
                mMediaPlayer.SetOnCompletionListener(this);//设置完成监听

                mMediaPlayer.Prepare();//缓冲数据
                mMediaPlayer.Start();//开始播放
            }
            catch(Exception ex)
            {
                mMediaPlayer.Reset();
                mMediaPlayer.Dispose();
                Android.Widget.Toast.MakeText(MainActivity.Instance, "开始播放出错：" + ex, Android.Widget.ToastLength.Long).Show();
            }
        }

        public void OnCompletion(MediaPlayer mp)
        {
            var activity = MainActivity.Instance;
            activity.RunOnUiThread(() =>
            {
                _btn.Text = _name;
                _btn.IsEnabled = true;
            });

            try
            {
                //播放完释放资源
                if (mp != null)
                {
                    mp.Stop();
                    mp.Reset();
                    mp.Dispose();
                    mp = null;
                }
            }
            catch (Exception ex)
            {
                activity.RunOnUiThread(() =>
                {
                    Android.Widget.Toast.MakeText(MainActivity.Instance, "播放完成出错：" + ex, Android.Widget.ToastLength.Long).Show();
                });
            }
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            ;
            //throw new NotImplementedException();
        }
    }
}