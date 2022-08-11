using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.admin.Controllers
{
    public class BaiController : BaseController
    {

        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public JsonResult List(string tuKhoa, int trang, int idMh)

        {
            try
            {

                var dsBai = (from b in db.Bais
                             join c in db.Chuongs on b.IdChuong equals c.Id
                             join m in db.MonHocs on c.IdMonHoc equals m.Id
                             where(b.DaXoa != 1 && b.TenBai.ToLower().Contains(tuKhoa) && m.Id == idMh)
                             select new
                             {
                                 Id = b.Id,
                                 Chuong = c.TenChuong,
                                 TenBai = b.TenBai,
                                 Meta = b.Meta
                             }
                                ).ToList().OrderBy(x=>x.Chuong).ToList();


                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = dsBai.Count() % pageSize == 0 ? dsBai.Count() / pageSize : dsBai.Count() / pageSize + 1;

                var kqht = dsBai
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();


                return Json(new { code = 200, soTrang = soTrang, dsBh = kqht, msg = "Lấy danh sách bài học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách bài học thất bại: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }


        //hàm thêm mới môn học
        [HttpPost]
        public JsonResult Add(string tenBai, string meta, int idChuong)
        {
            try
            {
                var b = new Bai();
                b.TenBai = tenBai;
                b.Meta = meta;
                b.IdChuong = idChuong;
                db.Bais.Add(b);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm mới bài học thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới bài học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;//cấu hình proxy cho database
                var b = db.Bais.SingleOrDefault(x => x.Id == id);
                return Json(new { code = 200, b = b }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin bài học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Edit(int id, string tenBai, string meta, int idChuong)
        {
            try
            {
                //tìm ra môn học cần cập nhật dựa vào id truyền vào
                var b = db.Bais.SingleOrDefault(x => x.Id == id);

                b.TenBai = tenBai;
                b.Meta = meta;
                b.IdChuong = idChuong;

                //luu vao csdl
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật bài học thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật bài học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var b = db.Bais.SingleOrDefault(x => x.Id == id);
                b.DaXoa = 1;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa bài học thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Xóa bài học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}