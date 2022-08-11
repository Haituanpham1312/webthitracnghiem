using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class DayController : BaseController
    {


        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }


        //hàm lấy danh sách giảng dạy
        [HttpGet]
        public JsonResult List(string tuKhoa,int trang)
        {
            try
            {
                var rs = (from d in db.Days.Where(x => x.DaXoa != 1)
                          join gv in db.GiaoViens on d.IdGiaoVien equals gv.Id
                          join mh in db.MonHocs on d.IdMonHoc equals mh.Id
                          join l in db.Lops on d.IdLop equals l.Id
                          select new
                          {
                              Id = d.Id,
                              TenGv = gv.HoTen,
                              TenMh = mh.TenMonHoc,
                              TenLop = l.TenLop,
                              TuNgay = d.TuNgay.Value.Day + "/" + d.TuNgay.Value.Month + "/" + d.TuNgay.Value.Year,
                              ToiNgay = d.ToiNgay.Value.Day + "/" + d.ToiNgay.Value.Month + "/" + d.ToiNgay.Value.Year
                          }).ToList()
                          .Where(x => x.TenGv.ToLower().Contains(tuKhoa) || x.TenMh.ToLower().Contains(tuKhoa))
                          .OrderBy(x => x.TenGv)
                          .OrderBy(y => y.TenLop)
                          .ToList();                        

                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = rs.Count() % pageSize == 0 ? rs.Count() / pageSize : rs.Count() / pageSize + 1;

                var kqht = rs
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
                return Json(new { code = 200, soTrang = soTrang, dsLop = kqht, msg = "Lấy danh sách giảng dạy thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách giảng dạy thất bại: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }


        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                var rs = (from x in db.Days
                          where (x.Id == id)
                          select new {
                              Id = x.Id,
                              IdGv = x.IdGiaoVien,
                              IdMh = x.IdMonHoc,
                              IdLop = x.IdLop,
                              TuNgay = (x.TuNgay.Value.Day < 10 ? string.Format("0{0}",x.TuNgay.Value.Day):string.Format("{0}",x.TuNgay.Value.Day))
                                        +"/"+( x.TuNgay.Value.Month < 10 ? string.Format("0{0}", x.TuNgay.Value.Month) : string.Format("{0}", x.TuNgay.Value.Month))
                                        +"/"+x.TuNgay.Value.Year, //--->dd/mm/yyyy
                            ToiNgay = (x.ToiNgay.Value.Day < 10 ? string.Format("0{0}", x.ToiNgay.Value.Day) : string.Format("{0}", x.ToiNgay.Value.Day))
                                        + "/" + (x.ToiNgay.Value.Month < 10 ? string.Format("0{0}", x.ToiNgay.Value.Month) : string.Format("{0}", x.ToiNgay.Value.Month))
                                        + "/" + x.ToiNgay.Value.Year, //--->dd/mm/yyyy
                          }).ToList();
               
                return Json(new { code = 200, msg = "Lấy chi tiết quá trình giảng dạy thành công",d= rs.Count>0?rs[0]:null }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy chi tiết quá trình giảng dạy thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //hàm thêm 1 quá trình giảng dạy
        public JsonResult AddDay(int maGv, int maLop,int maMh,string tuNgay,string toiNgay)
        {
            try
            {
                var frd = DateTime.ParseExact(tuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var td = DateTime.ParseExact(toiNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //khoi tao 1 object thuoc class Day
                var day = new Day();

                //gan thuoc tinh cho object vua tao bang cac gia tri truyen vao
                day.IdGiaoVien = maGv;
                day.IdLop = maLop;
                day.IdMonHoc = maMh;
                day.TuNgay = frd;
                day.ToiNgay = td;

                //add object dc tao o tren vao ds day
                db.Days.Add(day);

                //lu vao csdl
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới quá trình giảng dạy thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới quá trình giảng dạy thất bại: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //hàm cập nhật quá trình giảng dạy
        public JsonResult EditDay(int id,int maGv,int maLop, int maMh, string tuNgay, string toiNgay)
        {
            try
            {

                var frd = DateTime.ParseExact(tuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var td = DateTime.ParseExact(toiNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //tim ra thang can update dua vao id truyen vao
                var day = db.Days.SingleOrDefault(x => x.Id == id);

                //gan thuoc tinh cho object vua tao bang cac gia tri truyen vao
                day.IdGiaoVien = maGv;
                day.IdLop = maLop;
                day.IdMonHoc = maMh;
                day.TuNgay = frd;
                day.ToiNgay = td;
               
                //lu vao csdl
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật quá trình giảng dạy thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật quá trình giảng dạy thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //hàm xóa 1 quá trình giảng dạy
        public JsonResult DeleteDay(int id)
        {
            try
            {

                //tim ra thang can update dua vao id truyen vao
                var day = db.Days.SingleOrDefault(x => x.Id == id);
                //cập nhật lại trạng thái đã xóa = 1 <=> đã xóa
                day.DaXoa = 1;             
                //lu vao csdl
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa quá trình giảng dạy thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa quá trình giảng dạy thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}