using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace mmx.Models
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Update { get; set; }
    }
}
