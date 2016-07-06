using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspnetmvc5.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult Mvc()
        {
            return View();
        }

        public ActionResult WebApi()
        {
            return View();
        }

        public ActionResult HttpClient()
        {
            return View();
        }
    }
}