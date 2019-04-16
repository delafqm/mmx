using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace mmx.Models
{
    public class Words
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        //唯一编号，生成后不变
        public string Gid { get; set; }
        //上级编号
        public string Sid { get; set; }
        //名称
        public string Name { get; set; }
        //内容
        public string Content { get; set; }
        //类型：W-单词，S-语句
        public string Type { get; set; }
        //上传：S-上传成功，F-上传失败，N-未上传
        public string Upload { get; set; }
        //上传时间
        public DateTime UploadDate { get; set; }
    }
}
