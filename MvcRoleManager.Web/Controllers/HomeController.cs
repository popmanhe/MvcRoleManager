using MvcRoleManager.Security;
using MvcRoleManager.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcRoleManager.Controllers
{
    [Security.Attributes.Description("")]
    [Authorize]
    public class HomeController : Controller
    {
        [Security.Attributes.Description("Home page")]
        public ActionResult Index()
        {
            
            ControllersActions ca = new ControllersActions();
            List <MvcController> pclist = ca.GetControllers(true);
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
