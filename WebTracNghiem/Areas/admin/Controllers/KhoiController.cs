using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class KhoiController : BaseController
    {
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult AllKhoi()
        {
            try
            {
                var allKhoi = (from k in db.Khois
                        select new
                        {
                            Id = k.Id,
                            TenKhoi = k.TenKhoi
                        }).ToList();
                return Json(new { code = 200, allKhoi = allKhoi, msg = "Load danh sách khối thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Load danh sách khối thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}