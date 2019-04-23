using Microsoft.EntityFrameworkCore;
using mmx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mmx.MobileAppService.Models
{
    public class DataContext : DbContext
    {
        //public DataContext(DbContextOptions<DataContext> options) : base(options)
        //{ }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=47.99.36.29;Database=mmx;Uid=root;Pwd=123456;SslMode=none;");
            //optionsBuilder.UseSqlServer("Data Source=192.168.0.3; database=mmx; uid=sa; pwd=delafqm; MultipleActiveResultSets=True;");
        }

        public DbSet<TestItem> TestItems { get; set; }

        public DbSet<Lessons> Lessons { get; set; }

        public DbSet<Setting> Settings { get; set; }
    }
}
