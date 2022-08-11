using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.teacher.Controllers
{
    public class CauHoiController : BaseController
    {
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListMDK()
        {
            try
            {
                var rs = (
                    from m in db.MucDoKhos.Where( x=>x.DaXoa != 1 )
                   
                    select new
                    {
                        Id = m.Id,
                        TenMDK = m.TenMucDo
                    }
                    ).ToList();
                return Json(new { code = 200, dsMDK = rs, msg = "Lấy danh sách mdk thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Lấy danh sách mdk thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

       [HttpPost]
        public JsonResult AddCauHoi(int idDeThi, int idCauHoi)
        {
            try
            {
                //kiểm tra đã tồn tại 1 record trong bảng chi tiết đề thi có idDethi và idCauhoi này chưa
                var kt = db.DeThi_ChiTiets.Count(x => x.IdCauHoi == idCauHoi && x.IdDeThi == idDeThi);
                if(kt > 0)
                {
                    return Json(new { code = 403, msg = "Câu hỏi này đã tồn tại trong đề thi!" }, JsonRequestBehavior.AllowGet);
                }
                var ctdt = new DeThi_ChiTiets();
                ctdt.IdCauHoi = idCauHoi;
                ctdt.IdDeThi = idDeThi;
                db.DeThi_ChiTiets.Add(ctdt);
                db.SaveChanges();
                return Json(new { code = 201, msg = "Thêm câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ListDTCT(int idDethi)
        {
            try
            {
                var rs = (
                    from ct in db.DeThi_ChiTiets.Where(x => x.IdDeThi == idDethi)
                    join ch in db.CauHois on ct.IdCauHoi equals ch.Id
                    join m in db.MucDoKhos on ch.IdMucDo equals m.Id
                    join d in db.DapAns on ch.IdDapAn equals d.Id
                    select new
                    {
                        Id = ct.IdCauHoi,
                        CauHoi = ch.Cauhoi,
                        DA = ch.dap_an_a,
                        DB = ch.dap_an_b,
                        DC = ch.dap_an_c,
                        DD = ch.dap_an_d,
                        MDK = m.TenMucDo,
                        DAD = d.DapAnDung

                       }
                    ).ToList();
                return Json(new { code = 200, dsCauHoi = rs, msg = "Load chi tiết đề thi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Load chi tiết đề thi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ListCauHoi(int idBai , int idMDK)
        {
            try
            {
                var rs = (
                    from c in db.CauHois.Where(x => x.IdBai == idBai && x.DaXoa != 1 && x.DaPheDuyet ==1 && x.IdMucDo == idMDK)
                    join d in db.DapAns on c.IdDapAn equals d.Id
                  
                          select new
                          {
                              Id = c.Id,
                              CauHoi = c.Cauhoi,
                              DA = c.dap_an_a,
                              DB = c.dap_an_b,
                              DC = c.dap_an_c,
                              DD = c.dap_an_d,
                              DAD = d.DapAnDung,
                            

                          }
                           ).ToList();
                return Json(new { code = 200, dsCH = rs, msg = "Lấy danh sách câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ListBai(int idChuong)
        {
            try
            {
                var rs = (from b in db.Bais.Where(x => x.IdChuong == idChuong && x.DaXoa != 1)
                          select new
                          {
                              Id = b.Id,
                              TenBai = b.TenBai

                          }
                           ).ToList();
                return Json(new { code = 200, dsBai = rs, msg = "Lấy danh sách bài thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Lấy danh sách bài thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListChuong(int idMH)
        {
            try
            {
                var rs = (from ch in db.Chuongs.Where(x => x.IdMonHoc == idMH && x.DaXoa != 1)
                          select new
                          {
                              Id = ch.Id,
                              TenChuong = ch.TenChuong

                          }
                           ).ToList();
                return Json(new { code = 200, dsCH = rs, msg = "Lấy danh sách chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Lấy danh sách chương thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(int idBai, string cauHoi, string dA, string dB, string dC, string dD, int dAD, int MDK, string ghiChu)
        {
            try
            {
                var gv = (GiaoVien)Session["teacher"];
                // khai bao 1 object thuoc class CauHoi
                var ch = new CauHois();

                //gan cac thuoc tinh duoc truyen vao
                ch.IdBai = idBai;
                ch.Cauhoi = cauHoi;
                ch.dap_an_a = dA;
                ch.dap_an_b = dB;
                ch.dap_an_c = dC;
                ch.dap_an_d = dD;
                ch.IdDapAn = dAD;
                ch.IdMucDo = MDK;
                ch.ghi_chu = ghiChu;
                ch.NguoiTao = gv.Id;
                ch.NgayTao = DateTime.Now;


                db.CauHois.Add(ch);
                db.SaveChanges();

                return Json(new { code = 201, msg = "Thêm mới câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Thêm mới câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Edit(long id, int idBai, string cauHoi, string dA, string dB, string dC, string dD, int dAD, int MDK, string ghiChu)
        {
            try
            {
                var gv = (GiaoVien)Session["teacher"];
                // khai bao 1 object thuoc class CauHoi
                var ch = db.CauHois.SingleOrDefault(x => x.Id == id);

                //gan cac thuoc tinh duoc truyen vao
                ch.IdBai = idBai;
                ch.Cauhoi = cauHoi;
                ch.dap_an_a = dA;
                ch.dap_an_b = dB;
                ch.dap_an_c = dC;
                ch.dap_an_d = dD;
                ch.IdDapAn = dAD;
                ch.IdMucDo = MDK;
                ch.ghi_chu = ghiChu;
                ch.NguoiSua = gv.Id;
                ch.NgaySua = DateTime.Now;

                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "Cập nhật câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var gv = (GiaoVien)Session["teacher"];
                var ch = db.CauHois.SingleOrDefault(x => x.Id == id);
                ch.DaXoa = 1;
                ch.NgayXoa = DateTime.Now;
                ch.NguoiXoa = gv.Id;

                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Apply(int id)
        {
            try
            {
                var gv = (GiaoVien)Session["teacher"];
                var c = db.CauHois.SingleOrDefault(x => x.Id == id);
                c.DaPheDuyet = 1;
                c.NguoiPheDuyet = gv.Id;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Phê duyệt câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Phê duyệt câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult List(string tuKhoa, int trang, int idMH)
        {
            try
            {
                var gv = (GiaoVien)Session["teacher"];
                var isTBM = db.MonHocs.SingleOrDefault(x => x.Id == idMH).IdTBM == gv.Id ? true : false;


                var dsCH = (
                                from c in db.CauHois.Where(
                                        x => x.DaXoa != 1 &&
                                        (
                                            x.Cauhoi.ToLower().Contains(tuKhoa)
                                            || x.dap_an_a.ToLower().Contains(tuKhoa)
                                             || x.dap_an_b.ToLower().Contains(tuKhoa)
                                              || x.dap_an_c.ToLower().Contains(tuKhoa)
                                               || x.dap_an_d.ToLower().Contains(tuKhoa)
                                                || x.ghi_chu.ToLower().Contains(tuKhoa)
                                        ))

                                join b in db.Bais on c.IdBai equals b.Id
                                join ch in db.Chuongs.Where(x => x.IdMonHoc == idMH) on b.IdChuong equals ch.Id

                                #region thong_tin_chinh_sua_phe_duyet_xoa
                                join nt in db.GiaoViens on c.NguoiTao equals nt.Id
                                join ns in db.GiaoViens on c.NguoiSua equals ns.Id into ps
                                from ns in ps.DefaultIfEmpty()
                                join npd in db.GiaoViens on c.NguoiPheDuyet equals npd.Id into ss
                                from npd in ss.DefaultIfEmpty()
                                    #endregion

                                select new
                                {
                                    Id = c.Id,
                                    CauHoi = c.Cauhoi,
                                    NguoiTao = nt.HoTen,
                                    NgayTao = c.NgayTao,
                                    IdNguoiTao = nt.Id,
                                    NguoiSua = ns.HoTen,
                                    NgaySua = c.NgaySua,
                                    NguoiDuyet = npd.HoTen,
                                    DaDuyet = c.DaPheDuyet
                                }
                            ).ToList();


                var pageSize = int.Parse(db.CauHinhs.SingleOrDefault(x => x.TuKhoa == "so_dong_moi_trang").GiaTri);

                var soTrang = dsCH.Count() % pageSize == 0 ? dsCH.Count() / pageSize : dsCH.Count() / pageSize + 1;

                var kqht = dsCH
                            .Skip((trang - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();


                return Json(new { code = 200, dsCH = kqht, soTrang = soTrang, isTBM = isTBM, idDangNhap = gv.Id, msg = "Lấy danh sách câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }


        [HttpGet]
        public JsonResult ListUnapply(int idMH)
        {
            try
            {
                var rs = (from c in db.CauHois.Where(x => x.DaPheDuyet != 1 && x.DaXoa != 1)
                          join b in db.Bais on c.IdBai equals b.Id
                          join ch in db.Chuongs.Where(x => x.IdMonHoc == idMH) on b.IdChuong equals ch.Id
                          join mdk in db.MucDoKhos on c.IdMucDo equals mdk.Id
                          join dad in db.DapAns on c.IdDapAn equals dad.Id
                          select new
                          {
                              Id = c.Id,
                              CauHoi = c.Cauhoi,
                              DA = c.dap_an_a,
                              DB = c.dap_an_b,
                              DC = c.dap_an_c,
                              DD = c.dap_an_d,
                              GhiChu = c.ghi_chu,
                              MDK = mdk.TenMucDo,
                              DAD = dad.DapAnDung
                          }).ToList();
                return Json(new { code = 200, dsCH = rs, msg = "Lấy danh sách câu hỏi chưa phê duyệt thành công!" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi chưa phê duyệt thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }






        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                var rs = (from c in db.CauHois
                          join b in db.Bais on c.IdBai equals b.Id
                          where (c.Id == id)
                          select new
                          {
                              CauHoi = c.Cauhoi,
                              DA = c.dap_an_a,
                              DB = c.dap_an_b,
                              DC = c.dap_an_c,
                              DD = c.dap_an_d,
                              GhiChu = c.ghi_chu,
                              DAD = c.IdDapAn,
                              MDK = c.IdMucDo,
                              IdBai = c.IdBai,
                              IdChuong = b.IdChuong
                          }).ToList();
                return Json(new { code = 200, msg = "Lấy thông tin câu hỏi thành công!", c = rs.Count() > 0 ? rs[0] : null }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin câu hỏi thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
















        [HttpGet]
        public JsonResult ListDAD()
        {
            try
            {
                var dad = (from d in db.DapAns
                           select new
                           {
                               Id = d.Id,
                               DapAnDung = d.DapAnDung
                           }).ToList();
                return Json(new { code = 200, dad = dad, msg = "Load danh sách đáp án đúng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Load danh sách đáp án đúng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ListKhoi()
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

        [HttpGet]
        public JsonResult ListMonHoc()
        {
            try
            {
                var gv = (GiaoVien)Session["teacher"];
                var rs = (from d in db.Days
                         .Where(
                                    x => x.IdGiaoVien == gv.Id
                                    && x.TuNgay <= DateTime.Now
                                    && x.ToiNgay >= DateTime.Now
                            )
                          join m in db.MonHocs on d.IdMonHoc equals m.Id
                          join k in db.Khois on m.IdKhoi equals k.Id
                          select new
                          {
                              Id = m.Id,
                              TenMh = k.TenKhoi + " - " + m.TenMonHoc// tên khối - tên môn
                          }).ToList();

                return Json(new { code = 200, allMh = rs, msg = "Lấy danh sách môn học theo khối thành công!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách môn học theo khối thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListChuongByMhId(int idMh)
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
                return Json(new { code = 500, msg = "Lấy danh sách chương thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ListBaiByIdChuong(int idChuong)
        {
            try
            {

                var rs = (from b in db.Bais.Where(x => x.IdChuong == idChuong && x.DaXoa != 1)
                          select new
                          {
                              Id = b.Id,
                              TenBai = b.TenBai,
                              Meta = b.Meta
                          }).ToList();
                return Json(new { code = 200, msg = "Lấy danh sách bài thành công!", dsBh = rs }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách bài thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        

    }
}