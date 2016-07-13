using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvcRoleManager.Web.Security.Models;
using MvcRoleManager.Web.Security.ViewModels;

namespace MvcRoleManager.Web.Security.Store
{
    public class SqlserverRolePermissionStore : IRolePermissionStore
    {
        public List<MvcController> ReadActionPermissions()
        {
            throw new NotImplementedException();
        }

        public void SaveActionPermissions(List<MvcController> actions)
        {
            throw new NotImplementedException();
        }
    }
}
