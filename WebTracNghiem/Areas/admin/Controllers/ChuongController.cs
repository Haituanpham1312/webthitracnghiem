using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class ChuongController : BaseController
    {
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Get(int idMh)
        {
            try
            {

                var rs = (from c in db.Chuongs.Where(x => x.IdMonHoc == idMh && x.DaXoa != 1)
                          select new
                          {
                              Id = c.Id,
                              TenChuong = c.TenChuong,
                              Meta = c.Meta
                          }).ToList();
                return Json(new { code = 200, msg = "Lấy danh sách chương thành công!", chuongs = rs }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 200, msg = "Lấy danh sách chương thất bại: "+ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
           try
            {
                var rs = (from c in db.Chuongs.Where(x => x.Id == id)
                          select new
                          {
                              Id = c.Id,
                              TenChuong = c.TenChuong,
                              Meta = c.Meta
                          }).ToList();
                return Json(new { code = 200, msg = "Lấy thông tin chương thành công!", chuong = rs.Count>0?rs[0]:null }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 200, msg = "Lấy thông tin chương thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Add(string tenChuong, string meta, int idMh)
        {
            try
            {
                var chuong = new Chuong();
                chuong.TenChuong = tenChuong;
                chuong.Meta = meta;
                chuong.IdMonHoc = idMh;
                db.Chuongs.Add(chuong);
                db.SaveChanges();
                return Json(new { code = 201, msg = "Chương được thêm thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chương mới thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, string tenChuong, string meta)
        {
            try
            {
                var chuong =db.Chuongs.SingleOrDefault(x=>x.Id == id);
                chuong.TenChuong = tenChuong;
                chuong.Meta = meta;                          
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thông tin chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thông tin chương  thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var chuong = db.Chuongs.SingleOrDefault(x => x.Id == id);
                chuong.DaXoa = 1;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Chương đã bị xóa!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa chương  thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}