using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mmx.MobileAppService.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace mmx.MobileAppService.Controllers
{
    
    public class LessonsController : Controller
    {
        public string _ConnStr { get; }

        public LessonsController(string connstr)
        {
            this._ConnStr = connstr;
        }

        [Route("api/GetLessonsListAll")]
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            IEnumerable<Lessons> list;
            using (var conn = new MySqlConnection(_ConnStr))
            {
                conn.Open();
                string sqlcommand = @"select * from lessons";

                list = await conn.QueryAsync<Lessons>(sqlcommand);

                conn.Close();
            }


            return Ok(list);
        }


        [Route("api/GetLastUpdate")]
        [HttpGet]
        public async Task<Setting> SettingByUpdate()
        {
            Setting setting;
            using (var conn = new MySqlConnection(_ConnStr))
            {
                conn.Open();

                string sqlcommand = @"select * from setting where Name=@Name";

                setting = await conn.QueryFirstOrDefaultAsync<Setting>(sqlcommand, new { Name = "lastupdate" });

                conn.Close();
            }

            return setting;
        }
    }
}