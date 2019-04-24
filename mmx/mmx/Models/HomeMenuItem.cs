using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mmx.Models
{
    public enum MenuItemType
    {
        [Description("人教版英语")]
        English,
        [Description("更新内容")]
        Updata,
        [Description("欢迎页面")]
        welcome
    }

    public enum PalySpeed
    {
        [Description("超慢")]
        SuperSlow,
        [Description("慢速")]
        Slow,
        [Description("正常")]
        Normal,
        [Description("快速")]
        Fast
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
