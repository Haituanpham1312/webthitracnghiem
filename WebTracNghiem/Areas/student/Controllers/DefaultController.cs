using System.Linq;
using System.Web.Mvc;
using WebTracNghiem.Models;

namespace WebTracNghiem.Areas.student.Controllers
{
    public class DefaultController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: admin/Default
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["student"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            var usr = username;
            var pwd = password;
            var acc = db.HocSinhs.SingleOrDefault(x => x.MaHocSinh == usr && x.MatKhau == pwd);
            if (acc != null)
            {
                //đăng nhập thành công
                Session["student"] = acc;
               return RedirectToAction("Index", "Home");
            }
            else
            {
                //đăng nhập thất bại
                return View();
            }

         
        }
    }
}