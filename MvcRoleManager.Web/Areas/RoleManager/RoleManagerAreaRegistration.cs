using System.Web.Mvc;

namespace MvcRoleManager.Web.Areas.RoleManager
{
    public class RoleManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RoleManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RoleManager_default",
                "RoleManager/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MvcRoleManager.Web.Areas.RoleManager.Controllers" }
            );
        }
    }
}