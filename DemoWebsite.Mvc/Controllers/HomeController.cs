using System;
using System.Web.Mvc;

namespace DemoWebsite.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Partial()
        {
            return PartialView("Partial");
        }

        public ActionResult Json()
        {
            return Json(new {timestamp = DateTime.Now.ToString()}, JsonRequestBehavior.AllowGet);
        }
    }
}
