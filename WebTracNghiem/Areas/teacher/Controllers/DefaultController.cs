using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;
using System.Data.Entity;
using System.Linq;

namespace WebTracNghiem.Areas.teacher.Controllers
{
    public class DefaultController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: admin/Default
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["teacher"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var rs = (from gv in db.GiaoViens.Where(x => x.MaGiaoVien == username && x.MatKhau == password && x.DaXoa != 1)
                      join d in db.Days.Where(y => y.TuNgay <= DateTime.Now && y.ToiNgay >= DateTime.Now)
                      on gv.Id equals d.IdGiaoVien
                      select new 
                      {
                          Id = gv.Id,
                          HoTen = gv.HoTen
                      }).ToList();


            if(rs == null || rs.Count == 0)
            {
                return View();
            }else
            {
                var gv = new GiaoVien()
                {
                    Id = rs[0].Id,
                    HoTen = rs[0].HoTen
                };
                //đăng nhập thành công
                Session["teacher"] = gv;
                return RedirectToAction("Index", "Home");
            }
          



        }
    }
}