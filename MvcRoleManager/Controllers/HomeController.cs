using MvcRoleManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcRoleManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ControllersActions ca = new ControllersActions();
            List < ProjectController> pclist = ca.GetControllers(Server.MapPath("~/bin/" + "MvcRoleManager.Web.dll"));
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
