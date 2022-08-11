using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTracNghiem.Areas.student.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["student"] == null)//chua đăng nhập admin
            {
                //thì trả về trang đăng nhập của admin
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { Controller = "Default", Action = "Login" })
                );
            }
            base.OnActionExecuting(filterContext);
        }
    }
}