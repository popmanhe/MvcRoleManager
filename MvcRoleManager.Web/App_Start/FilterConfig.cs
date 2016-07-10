using RoleSecurityManager.Security;
using System.Web.Mvc;

namespace MvcRoleManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcAuthoraiztionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
