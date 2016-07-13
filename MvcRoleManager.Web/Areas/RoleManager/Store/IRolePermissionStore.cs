using MvcRoleManager.Web.Security.Models;
using MvcRoleManager.Web.Security.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Web.Security.Store
{
    interface IRolePermissionStore
    {
        void SaveActionPermissions(List<MvcController> actions);

        List<MvcController> ReadActionPermissions();

    }

    interface IRolePermissions
    {
        List<string> GetRoles(MethodInfo method);

    }
}

