using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRoleManager.Web.Areas.Security.Controllers
{
    public class RoleManagerController : Controller
    {
        // GET: Security/RoleManager
        public ActionResult Index()
        {
            return View();
        }
    }
}