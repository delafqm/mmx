using Xamarin.Forms;

namespace mmx
{
    public interface ITextToSpeech
    {

        /// <summary>
        /// 根据文本播放语音
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="speed">播放语速</param>
        /// <param name="pitch">播放音高</param>
        /// <param name="btn">设置按钮</param>
        /// <param name="name">设置内容</param>
        void Speak(string text, float speed, float pitch, Button btn, string msg);
    }
}
