using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mmx.MobileAppService.Models;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmx.MobileAppService.Controllers
{
    
    public class LessonsController : Controller
    {
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

        #region EF方式
        private DataContext db = new DataContext();

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
            var selectlist = db.Lessons.Where(t => t.Types == "G");
            var selectlist1 = from t in selectlist select new SelectItem { Value = t.Gid, Text = t.Name };
            ViewBag.TypeList = new SelectList(selectlist1, "Value", "Text");

            return View();
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Lessons lessons = new Lessons();
                var selectlist = db.Lessons.Where(t => t.Types == "G");
                //var selectlist1 = from t in selectlist select new SelectItem { Value = t.Gid, Text = t.Name };
                List<SelectItem> selectlist2 = new List<SelectItem>();
                selectlist2.Add(new SelectItem { Value = null, Text = "顶层" });
                foreach(var item in selectlist)
                {
                    selectlist2.Add(new SelectItem { Value = item.Gid, Text = item.Name });
                }

                ViewBag.TypeList = new SelectList(selectlist2, "Value", "Text");


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
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}