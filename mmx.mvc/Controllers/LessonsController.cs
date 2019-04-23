using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmx.mvc.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace mmx.mvc.Controllers
{
    public class LessonsController : Controller
    {
        private DataContext db = new DataContext();


        #region ado.net方式
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
        #endregion

        // GET: Item
        public ActionResult Index()
        {
            //var lists = from u in db.Movie select u;

            return View(db.Lessons);
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            Lessons lessons = db.Lessons.Find(id);
            return View(lessons);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.TypeList = new SelectList(GetTypeList(), "Value", "Text");

            ViewBag.SidList = new SelectList(GetSelectItems(), "Value", "Text");
            
            return View();
        }

        /// <summary>
        /// 初始化下拉框数据--上级ID
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetSelectItems()
        {
            var selectlist = db.Lessons.Where(t => t.Types == "G");
            //var selectlist1 = from t in selectlist select new SelectItem { Value = t.Gid, Text = t.Name };
            List<SelectListItem> selectlist2 = new List<SelectListItem>();
            selectlist2.Add(new SelectListItem { Value = null, Text = "顶层" });
            foreach (var item in selectlist)
            {
                selectlist2.Add(new SelectListItem { Value = item.Gid, Text = item.Name });
            }

            return selectlist2;
        }

        /// <summary>
        /// 绑定下拉框选定数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<SelectListItem> SetSelectItems(List<SelectListItem> list, string value)
        {
            foreach (var item in list)
            {
                if (item.Value == value)
                {
                    item.Selected = true;
                }
            }

            return list;
        }

        /// <summary>
        /// 初始化下拉框数据--类型
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetTypeList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Value = "G", Text = "年级" });
            list.Add(new SelectListItem { Value = "L", Text = "课号" });
            list.Add(new SelectListItem { Value = "S", Text = "语句" });
            list.Add(new SelectListItem { Value = "W", Text = "单词" });
            return list;
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Lessons lessons = new Lessons();

                lessons.Gid = Guid.NewGuid().ToString();
                lessons.Sid = collection["Sid"];
                lessons.Name = collection["Name"];
                lessons.Text = collection["Text"];
                lessons.Types = collection["Types"];

                db.Lessons.Add(lessons);

                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            Lessons lessons = db.Lessons.Find(id);

            ViewBag.TypeList = new SelectList(SetSelectItems(GetTypeList(),lessons.Types), "Value", "Text");
            ViewBag.SidList = new SelectList(SetSelectItems(GetSelectItems(), lessons.Sid), "Value", "Text");

            return View(lessons);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Lessons lessons = db.Lessons.Find(id);

                lessons.Name = collection["Name"];
                lessons.Text = collection["Text"];
                lessons.Types = collection["Types"];

                db.Entry(lessons).State = EntityState.Modified;

                //修改后设置内容已更新
                Setting setting = db.Settings.Where(o => o.Name == "lastupdate").FirstOrDefault();
                setting.Update = DateTime.Now;
                db.Entry(setting).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            Lessons movie = db.Lessons.Find(id);

            return View(movie);
        }

        // POST: Item/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Lessons lessons = db.Lessons.Find(id);
                db.Lessons.Remove(lessons);
                //删除后设置内容更新
                Setting setting = db.Settings.Where(o => o.Name == "lastupdate").FirstOrDefault();
                setting.Update = DateTime.Now;
                db.Entry(setting).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}