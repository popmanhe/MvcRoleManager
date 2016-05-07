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
            //ControllersActions ca = new ControllersActions();
            //List <MvcController> pclist = ca.GetControllers(true);
            ViewBag.Title = "Home Page";

            return View();
        }
        //[AllowAnonymous]
        [Route("edit/{id:int}")]
        [HttpGet]
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
