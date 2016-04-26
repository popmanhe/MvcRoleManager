﻿using RoleSecurityManager.Security;
using System.Web;
using System.Web.Mvc;

namespace MvcRoleManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcAuthorization());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
