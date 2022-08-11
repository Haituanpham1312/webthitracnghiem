using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class HocSinhController : BaseController
    {
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult AllHs()
        {
            try
            {
                var dsHv = (from hs in db.HocSinhs.Where(x => x.DaXoa != 1)
                            select new
                            {
                                Id = hs.MaHocSinh,
                                HoTen = hs.HoTen,
                                Email = hs.Email,
                                DienThoai = hs.DienThoai,
                                DiaChi = hs.DiaChi

                            }).ToList();

                return Json(new { code = 200, dsHv = dsHv }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách học sinh thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult SelectHs(string tuKhoa, int trang)
        {
            try
            {
                var dsHs = (from hs in db.HocSinhs
                            .Where(x => x.DaXoa != 1 //laays nhung hs chua bi xoa
                                && (
                                x.HoTen.ToLower().Contains(tuKhoa) //ho ten co chua tu khoa
                                || x.DienThoai.ToLower().Contains(tuKhoa) //hoac la sodienthoai co chua tu khoa
                                || x.Email.ToLower().Contains(tuKhoa)//email co chua tu khoa
                                || x.DiaChi.ToLower().Contains(tuKhoa)//hoac dia chi co chua tu khoa
                                || x.MaHocSinh.ToLower().Contains(tuKhoa)//hoac la magv co chua tu khoa
                                )
                            )
                            select new
                            {
                                Id = hs.MaHocSinh,
                                HoTen = hs.HoTen,
                                NgaySinh = hs.NgaySinh,
                                Email = hs.Email,
                                DienThoai = hs.DienThoai,
                                DiaChi = hs.DiaChi
                            }).ToList();


                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = dsHs.Count() % pageSize == 0 ? dsHs.Count() / pageSize : dsHs.Count() / pageSize + 1;

                var kqht = dsHs
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
                return Json(new { code = 200, soTrang = soTrang, dsHs = kqht, msg = "Lấy danh sách học sinh thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách học sinh thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LayHs(string mhs)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;//cấu hình proxy cho database
                var hs = db.HocSinhs.SingleOrDefault(x => x.MaHocSinh == mhs);
                
                return Json(new { code = 200, hs = hs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chi tiết của học sinh thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddHs(string maHs, string matKhau, string hoTen,string ngaySinh, string soDienThoai, string email, string diaChi)
        {
            try
            {
                var chk = db.HocSinhs.Where(x => x.MaHocSinh == maHs).Count() == 0;
                if (!chk)
                {
                    return Json(new { code = 300, msg = "Mã học sinh này đã tồn tại trong hệ thống" }, JsonRequestBehavior.AllowGet);
                }
                var hs = new HocSinh();


                hs.MaHocSinh = maHs;
                hs.MatKhau = matKhau;
                hs.HoTen = hoTen;
                hs.NgaySinh = DateTime.Parse(ngaySinh);
                hs.DienThoai = soDienThoai;
                hs.Email = email;
                hs.DiaChi = diaChi;             

                db.HocSinhs.Add(hs);

                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới học sinh thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới học sinh thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditHs(string maHs, string matKhau, string hoTen, string ngaySinh, string soDienThoai, string email, string diaChi)
        {
            try
            {
                var hs = db.HocSinhs.SingleOrDefault(x => x.MaHocSinh == maHs);

                hs.MaHocSinh = maHs;
                hs.MatKhau = matKhau;
                hs.HoTen = hoTen;
                hs.NgaySinh = DateTime.Parse(ngaySinh);
                hs.DienThoai = soDienThoai;
                hs.Email = email;
                hs.DiaChi = diaChi;

                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật thông tin học sinh thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thông tin học sinh thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult DeleteHs(string maHs)
        {
            try
            {
                var hs = db.HocSinhs.SingleOrDefault(x => x.MaHocSinh == maHs);
                hs.DaXoa = 1;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa học sinh thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa học sinh thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}