using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRoleManager.Web.Areas.RoleManager.Controllers
{
    public class HomeController : Controller
    {
        // GET: RoleManager/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Users()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Controllers()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult SimpleRole()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ActionRole()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult RoleAction()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Roles()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult RoleUser()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult UserRole()
        {
            return PartialView();
        }
    }
}