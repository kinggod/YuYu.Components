﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YuYu.Components;

namespace MvcTest.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [Authority(Display = "")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
