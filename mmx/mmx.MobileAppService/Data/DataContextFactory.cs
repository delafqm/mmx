using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using mmx.MobileAppService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace mmx.MobileAppService.Data
{
    public class DataContextFactory: IDesignTimeDbContextFactory<DataContext>
    {
        //IConfiguration Configuration { get; } //使用Configuration 获取不到GetConnectionString("SchoolContext")。不能用
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            //optionsBuilder.UseMySQL(Configuration["DB:mysqlDB"]);
            optionsBuilder.UseMySQL("Server=47.99.36.29;Database=mmx;Uid=root;Pwd=123456;SslMode=none;");
            //return new DataContext(optionsBuilder.Options);
            return new DataContext();
        }
    }
}
