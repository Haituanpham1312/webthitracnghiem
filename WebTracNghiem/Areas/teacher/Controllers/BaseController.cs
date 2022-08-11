using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTracNghiem.Areas.teacher.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["teacher"] == null)//chua đăng nhập giáo viên
            {
                //thì trả về trang đăng nhập của giáo viên
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { Controller = "Default", Action = "Login" })
                );
            }
            base.OnActionExecuting(filterContext);
        }
    }
}