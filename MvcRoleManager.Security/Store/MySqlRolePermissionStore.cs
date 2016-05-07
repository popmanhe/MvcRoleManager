using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvcRoleManager.Security.Model;

namespace MvcRoleManager.Security.Store
{
    public class MySqlRolePermissionStore : IRolePermissionStore
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
