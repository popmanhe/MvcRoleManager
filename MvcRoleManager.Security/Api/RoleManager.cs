using MvcRoleManager.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MvcRoleManager.Security.Api
{
   public class RoleManagerController: ApiController
    {
        private ControllersActions _controllersActions = new ControllersActions ();
        public List<MvcController> GetControllers() {
            return this._controllersActions.GetControllers(true);
        }
    }
}
