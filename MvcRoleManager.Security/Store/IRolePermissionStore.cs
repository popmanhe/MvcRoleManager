using MvcRoleManager.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Store
{
    interface IRolePermissionStore
    {
        List<MvcAction> GetMethods(string controllerName, string actionName);
        
    }

    public class RoleActions
        {
         public string ControllerName { get; set; }
    public string ActionName { get; set; }
    public string ActionJson { get; set; }
        public string Roles { get; set; }
}

}

