using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace MvcRoleManager.Controllers
{
    [Description("")]
    [Authorize]
    public class HomeController : Controller
    {
        [Description("Home page")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [Description("Login page")]
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }
        //[AllowAnonymous]
        [Route("edit/{id:int}")]
        [HttpGet,HttpOptions]
        //[ActionName("Edit")]
        public ActionResult Edit(int id)
        { 
            return View();
        }
       
        //[AllowAnonymous]
        [HttpGet]
        [Route("edit/{id:datetime}")]
        //[ActionName("Edit")]
        public ActionResult Edit(DateTime name)
        {
            return View();
        }
    }
}
