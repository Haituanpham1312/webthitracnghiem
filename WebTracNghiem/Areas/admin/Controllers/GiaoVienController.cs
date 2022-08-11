using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class GiaoVienController : BaseController
    {
        private DBEntities db = new DBEntities();


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult AllGiaoVien()
        {
            try
            {
                var dsGv = (from gv in db.GiaoViens.Where(x => x.DaXoa != 1)
                            select new
                            {
                                Id = gv.Id,
                                MaGv = gv.MaGiaoVien,
                                HoTen = gv.HoTen
                            }).ToList();

                return Json(new { code = 200, dsGv = dsGv }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách giáo viên thất bại: "+ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult SelectGiaoVien(string tuKhoa,int trang)
        {
            try
            {
                var dsGv = (from gv in db.GiaoViens
                            .Where(x => x.DaXoa != 1 //laays nhung gv chua bi xoa
                                &&(
                                x.HoTen.ToLower().Contains(tuKhoa) //ho ten co chua tu khoa
                                || x.SoDienThoai.ToLower().Contains(tuKhoa) //hoac la sodienthoai co chua tu khoa
                                || x.Email.ToLower().Contains(tuKhoa)//email co chua tu khoa
                                || x.DiaChi.ToLower().Contains(tuKhoa)//hoac dia chi co chua tu khoa
                                || x.MaGiaoVien.ToLower().Contains(tuKhoa)//hoac la magv co chua tu khoa
                                )
                            )
                            select new
                            {
                                Id = gv.Id,
                                MaGv = gv.MaGiaoVien,
                                HoTen = gv.HoTen,
                                SoDienThoai = gv.SoDienThoai,
                                Email = gv.Email,
                                DiaChi = gv.DiaChi,
                                TruongBM = 1
                            }).ToList();


                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = dsGv.Count() % pageSize == 0 ? dsGv.Count() / pageSize : dsGv.Count() / pageSize + 1;

                var kqht = dsGv
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
                return Json(new { code = 200, soTrang = soTrang, dsGv = kqht, msg = "Lấy danh sách giáo viên thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách giáo viên thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LayGV(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;//cấu hình proxy cho database
                var gv = db.GiaoViens.SingleOrDefault(x => x.Id == id);
                return Json(new { code = 200, gv = gv }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chi tiết của giáo viên thất bại: "+ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddGv(string maGv, string matKhau, string hoTen, string soDienThoai, string email, string diaChi, byte laTruongBM)
        {
            try
            {
                var gv = new GiaoVien();

                gv.MaGiaoVien = maGv;
                gv.MatKhau = matKhau;
                gv.HoTen = hoTen;
                gv.SoDienThoai = soDienThoai;
                gv.Email = email;
                gv.DiaChi = diaChi;
              

                db.GiaoViens.Add(gv);

                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới giáo viên thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới giáo viên thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditGv(int id,string maGv, string matKhau, string hoTen, string soDienThoai, string email, string diaChi, byte laTruongBM)
        {
            try
            {
                var gv = db.GiaoViens.SingleOrDefault(x=>x.Id == id);

                gv.MaGiaoVien = maGv;
                gv.MatKhau = matKhau;
                gv.HoTen = hoTen;
                gv.SoDienThoai = soDienThoai;
                gv.Email = email;
                gv.DiaChi = diaChi;
               

               db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật thông tin giáo viên thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thông tin giáo viên thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult DeleteGv(int id)
        {
            try
            {
                var gv = db.GiaoViens.SingleOrDefault(x => x.Id == id);
                gv.DaXoa = 1;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa giáo viên thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa giáo viên thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}