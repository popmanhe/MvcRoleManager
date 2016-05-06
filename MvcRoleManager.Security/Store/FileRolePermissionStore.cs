using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcRoleManager.Security.Model;

namespace MvcRoleManager.Security.Store
{
    public class FileRolePermissionStore : IRolePermissionStore
    {
        public List<MvcAction> GetMethods(string controllerName, string actionName)
        {
            throw new NotImplementedException();
        }
    }
}
