using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.teacher.Controllers
{
    public class DeThiController : BaseController
    {
        private DBEntities db = new DBEntities();
        // GET: teacher/DeThi
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public JsonResult TaoDeThi(int idMonHoc, string ngayThi, int nhomDeThi)
        {
            try
            {
                int idGV = (Session["teacher"] as GiaoVien).Id;
                var deThi = new DeThi();
                deThi.GvTao = idGV;
                deThi.NgayThi = DateTime.Parse(ngayThi);
                deThi.IdNhom = nhomDeThi;
                deThi.IdMonHoc = idMonHoc;

                db.DeThis.Add(deThi);
                db.SaveChanges();

                return Json(new { code = 201, msg = "Tạo mới đề thi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Tạo mới đề thi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadExamsList(int idMonHoc)
        {
            try
            {

                int idGV = (Session["teacher"] as GiaoVien).Id;

                var rs = (from d in db.DeThis.Where(x => x.GvTao == idGV && x.IdMonHoc == idMonHoc)
                          join n in db.NhomDeThis on d.IdNhom equals n.ID
                          select new
                          {
                              MaDT = d.Id,
                              NgayThi = d.NgayThi,
                              LoaiDe = n.TenNhom,
                              ThoiGian = n.ThoiGianThi,
                              TrangThai = d.DaDuyet,
                              DaXoa = d.DaXoa
                          }
                            ).ToList();


                return Json(new
                {
                    code = 200,
                    dsDT = rs,
                    msg = "Lấy danh sách đề thi thành công!"
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách đề thi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ListSubjects()
        {
            try
            {

                int idGV = (Session["teacher"] as GiaoVien).Id;

                var rs = (from d in db.Days.Where(x => x.IdGiaoVien == idGV)
                          join m in db.MonHocs on d.IdMonHoc equals m.Id
                          select new
                          {
                              TenMH = m.TenMonHoc,
                              IdMh = m.Id
                          }
                            ).ToList();


                return Json(new
                {
                    code = 200,
                    dsMH = rs,
                    msg = "Lấy danh sách môn giảng dạy thành công!"
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi chưa phê duyệt thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListExamGroup()
        {
            try
            {

                var rs = (from n in db.NhomDeThis
                          select new
                          {
                              TenNhom = n.TenNhom,
                              ID = n.ID
                          }
                            ).ToList();


                return Json(new
                {
                    code = 200,
                    dsNhom = rs,
                    msg = "Lấy danh sách nhóm đề thi thành công!"
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi chưa phê duyệt thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult List()
        {
            return null;
        }
    }
}