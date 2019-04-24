using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace mmx.Models
{
    public class Lessons
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Gid { get; set; }

        public string Sid { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Types { get; set; }
    }
}
