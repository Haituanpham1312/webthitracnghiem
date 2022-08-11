using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class NhapHocController : BaseController
    {
        private const int PAGE_SIZE = 2;
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult NhapHoc(String idLop, String MaHocSinh)
        {
            try
            {
                var h = new Hoc();
                h.NamHoc = DateTime.Now.Year;
                h.MaHocSinh = MaHocSinh;
                h.IdLop = int.Parse(idLop);

                db.Hocs.Add(h);
                db.SaveChanges();

                return Json(new { code = 200, h = h, msg = "Nhập học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Load danh sách hs thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DsHsDaNhapHoc(int namHoc, int trang, string tuKhoa)
        {
            try
            {
                var dshs = (from h in db.Hocs// bang dshs da duoc xep lop
                            join hs in db.HocSinhs on h.MaHocSinh equals hs.MaHocSinh// bang chua thong tin cua hoc sinh
                            where (h.NamHoc == namHoc &&
                                (
                                     hs.MaHocSinh.Contains(tuKhoa)
                                     || hs.HoTen.Contains(tuKhoa)
                                     || hs.DienThoai.Contains(tuKhoa)
                                     || hs.Email.Contains(tuKhoa)
                                )
                            )
                            select new
                            {
                                MaHs = hs.MaHocSinh,
                                HoTen = hs.HoTen,
                                NgaySinh = hs.NgaySinh,
                                DienThoai = hs.DienThoai,
                                DiaChi = hs.DiaChi,
                                Email = hs.Email
                            }
                            ).ToList() 
                           .ToList();
                var soTrang = dshs.Count() % PAGE_SIZE == 0 ? (dshs.Count / PAGE_SIZE) : (dshs.Count / PAGE_SIZE + 1);
                var kq = dshs.Skip((trang - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
                return Json(new { code = 200,soTrang = soTrang, dshs = kq, msg = "Load danh sách hs đã nhập học thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Load danh sách hs đã nhập học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult DsHSChuaXepLop(string tuKhoa, int page)
        {
            try
            {
                var DsHs = (from p in ((from hs in db.HocSinhs.Where(x => x.DaXoa != 1)
                          select new { Id = hs.MaHocSinh }).ToList()
                        .Except(from h in db.Hocs.Where(x => x.NamHoc == DateTime.Now.Year) select new { Id = h.MaHocSinh })
                        .ToList())
                       join q in db.HocSinhs on p.Id equals q.MaHocSinh
                       where(
                            q.MaHocSinh.Contains(tuKhoa)
                            || q.HoTen.Contains(tuKhoa)
                            || q.DienThoai.Contains(tuKhoa)
                            || q.Email.Contains(tuKhoa)
                       )
                       select new {
                           MaHocSinh = q.MaHocSinh,
                           HoTen = q.HoTen,
                           NgaySinh = q.NgaySinh,
                           DienThoai = q.DienThoai,
                           DiaChi = q.DiaChi,
                           Email = q.Email
                       }).ToList();
                var soTrang = DsHs.Count() % PAGE_SIZE == 0 ? (DsHs.Count / PAGE_SIZE) : (DsHs.Count / PAGE_SIZE + 1);
                var kq = DsHs.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
                return Json(new { code = 200,dshs = kq, soTrang = soTrang,  msg = "Load danh sách hs thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Load danh sách hs thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}