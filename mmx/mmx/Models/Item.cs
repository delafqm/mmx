using System;
using System.Collections.Generic;

namespace mmx.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 语音识别返回JSON实体
    /// </summary>
    public class MResult
    {
        public int err_no;
        public string err_msg;
        public List<string> result;
        public string sn;
    }

    /// <summary>
    /// 文本转语音，返回错误的数据类型
    /// </summary>
    public class TResult
    {
        public int err_no;
        public string err_msg;
        public int idx;
        public string sn;
    }

    public class SpeechResult
    {
        public int status;//状态，0为成功，1为失败
        public string error;//错误信息
        public byte[] speech;//文本转语音时的语音
        public string text;//语音转文本时的文本
    }

    /// <summary>
    /// 百度API--token
    /// </summary>
    public class BaiduToken
    {
        public string access_token { get; set; }
        public string session_key { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string session_secret { get; set; }
        public int expires_in { get; set; }
    }
}