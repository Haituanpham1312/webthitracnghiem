using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class MonHocController : BaseController
    {

        private DBEntities db = new DBEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult DsMonHoc(string tuKhoa, int trang)

        {
            try
            {

                var dsMonHoc = (from m in db.MonHocs
                             .Where(x => x.DaXoa != 1 && x.TenMonHoc.ToLower().Contains(tuKhoa))                              
                                select new
                                {
                                    Id = m.Id,
                                    TenMonHoc = m.TenMonHoc,
                                    Meta = m.Meta
                                }
                                ).ToList();


                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = dsMonHoc.Count() % pageSize == 0 ? dsMonHoc.Count() / pageSize : dsMonHoc.Count() / pageSize + 1;

                var kqht = dsMonHoc
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();


                return Json(new { code = 200, soTrang = soTrang, dsMonHoc = kqht, msg = "Lấy danh sách môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách môn học thất bại: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }


        //hàm thêm mới môn học
        [HttpPost]
        public JsonResult AddMonHoc(string tenMonHoc, string metaMonHoc)
        {
            try
            {
                //khai bao 1 object monhoc
                var mh = new MonHoc();

                //gan cac thuoc tinh cho object dc tao o tren
                mh.TenMonHoc = tenMonHoc;
                mh.Meta = metaMonHoc;            

                //add vao bang mon hoc
                db.MonHocs.Add(mh);

                //luu vao csdl
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới môn học thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới môn học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ChiTiet(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;//cấu hình proxy cho database
                var mh = db.MonHocs.SingleOrDefault(x => x.Id == id);
                return Json(new { code = 200, mh = mh}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500,msg = "Lấy thông tin lớp học thất bại: "+ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult EditMonHoc(int id,string tenMonHoc, string metaMonHoc)
        {
            try
            {
                //tìm ra môn học cần cập nhật dựa vào id truyền vào
                var mh = db.MonHocs.SingleOrDefault(x => x.Id == id);

                //gan cac thuoc tinh cho object dc tao o tren
                mh.TenMonHoc = tenMonHoc;
                mh.Meta = metaMonHoc;             

                //luu vao csdl
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật môn học thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật môn học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Xoa(int id)
        {
            try
            {               
                var mh = db.MonHocs.SingleOrDefault(x => x.Id == id);
                mh.DaXoa = 1;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa môn học thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Xóa môn học thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult AllMonHoc()
        {
            try
            {
                var allMh = (from mh in db.MonHocs.Where(x => x.DaXoa != 1)
                             select new
                             {
                                 Id = mh.Id,
                                 TenMH = mh.TenMonHoc
                             }).ToList();
                return Json(new { code = 200, allMh = allMh }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách môn học thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpGet]
        public JsonResult GetListByKhoiId(int idKhoi)
        {
            try
            {
                var dsMh = (from m in db.MonHocs.Where(x => x.IdKhoi == idKhoi && x.DaXoa != 1)
                            select new
                            {
                                Id = m.Id,
                                TenMh = m.TenMonHoc
                            }).ToList();
                return Json(new { code = 200, allMh = dsMh, msg = "Lấy danh sách môn học theo khối thành công!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách môn học theo khối thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}