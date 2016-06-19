using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Store
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

