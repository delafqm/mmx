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
}