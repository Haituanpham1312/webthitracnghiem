using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.teacher.Controllers
{
    public class ChuongController : BaseController
    {
        private DBEntities db = new DBEntities();
        [HttpGet]
        public JsonResult ListChuong(int idMH)
        {
            try
            {
                var dsChuong = db.Chuongs.Where(x=>x.IdMonHoc == idMH).ToList();
                return Json(new { code = 200, msg = "Lấy danh sách chương thành công",dsChuong = dsChuong, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

    }
}