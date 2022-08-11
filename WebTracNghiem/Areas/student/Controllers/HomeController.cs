﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTracNghiem.Areas.student.Controllers
{
    public class HomeController : BaseController
    {        
        public ActionResult Index()
        {          
            return View();
        }

        [ChildActionOnly]
        public ActionResult RenderMenu()
        {
            return PartialView("_Navbar");
        }
    }
}