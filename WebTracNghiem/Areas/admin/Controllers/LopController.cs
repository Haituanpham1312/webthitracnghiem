using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class LopController : BaseController
    {
        private DBEntities db = new DBEntities();

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult DsLop(string tuKhoa,int trang)
        {
            try
            {
                
                var dsLop = (from l in db.Lops
                             .Where(x => x.DaXoa != 1 && x.TenLop.ToLower().Contains(tuKhoa))
                             select new
                             {
                                 Id = l.Id,
                                 TenLop = l.TenLop,
                                 Meta = l.Meta
                             }).ToList();
                //30 dòng 40 dòng chẳng hạn ...


                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = dsLop.Count() % pageSize == 0? dsLop.Count()/ pageSize: dsLop.Count() / pageSize+1;

                var kqht = dsLop
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
                return Json(new { code = 200,soTrang = soTrang, dsLop = kqht, msg = "Lấy danh sách lớp thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500,msg =  "Lấy danh sách lớp thất bại: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }


        [HttpGet]
        public JsonResult AllLop()
        {
            try
            {
                var allLop = (from l in db.Lops.Where(x => x.DaXoa != 1)
                             select new
                             {
                                 Id = l.Id,
                                 TenLop = l.TenLop
                             }).ToList();
                return Json(new { code = 200, allLop = allLop, msg = "Load danh sách lớp thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500,  msg = "Load danh sách lớp thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult AllLopByKhoiId(int idKhoi)
        {
            try
            {
                var allLop = (from l in db.Lops.Where(x => x.DaXoa != 1 && x.IdKhoi == idKhoi)
                              select new
                              {
                                  Id = l.Id,
                                  TenLop = l.TenLop
                              }).ToList();
                return Json(new { code = 200, allLop = allLop, msg = "Load danh sách lớp thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Load danh sách lớp thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult AddLop(string tenLop,string meta)
        {
            try
            {
                var l = new Lop();
                l.TenLop = tenLop;
                l.Meta = meta;

                db.Lops.Add(l);//them doi tuong lop dc khai bao o phia tren
                db.SaveChanges();//luu vao csdl

                return Json(new { code = 200, msg = "Thêm lớp mới thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới lớp thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ChiTiet(int id)
        {
            try
            {
                var l = db.Lops.SingleOrDefault(x => x.Id == id);
                return Json(new { code = 200, L = l, msg = "Lấy thông tin chi tiết của lớp thành công" },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin lớp thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CapNhat(int id,string tenLop,string meta)
        {
            try
            {
                //tìm lớp dựa vào id
                var l = db.Lops.SingleOrDefault(x => x.Id == id);

                //gán lại các thuộc tính của lớp đc tìm thấy
                l.TenLop = tenLop;
                l.Meta = meta;

                //lưu lại csdl
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật lớp thành công!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật lớp thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult XoaLop(int id)
        {
            try
            {
                var l = db.Lops.SingleOrDefault(x => x.Id == id);
                l.DaXoa = 1;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa lớp học thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa lớp học thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
