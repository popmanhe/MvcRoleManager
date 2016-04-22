using MvcRoleManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcRoleManager.Controllers
{
   [ControllerDescription("")][Authorize]
    public class HomeController : Controller
    {
        [ActionDescription("")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ControllersActions ca = new ControllersActions(Server.MapPath("~/bin/" + "MvcRoleManager.Web.dll"));
            List <MvcController> pclist = ca.GetControllers();
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
