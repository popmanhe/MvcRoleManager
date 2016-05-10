using MvcRoleManager.Security.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.Security.Bso;

namespace MvcRoleManager.Security.Api
{
    [RoutePrefix("api/rolemanager")]
    public class RoleManagerController : ApiController
    {
        private RoleManagerBso _roleManagerBso;
        protected RoleManagerBso RoleManagerBso
        {
            get
            {
                this._roleManagerBso = this._roleManagerBso ?? new RoleManagerBso();
                return this._roleManagerBso;
            }
        }

        [AllowAnonymous]
        public List<MvcController> GetControllers()
        {
            return RoleManagerBso.GetControllers();
        }

        [AllowAnonymous]
        public List<IdentityRole> GetRoles()
        {
            return RoleManagerBso.GetRoles();
        }

        [AllowAnonymous]
        public Task<int> SaveActionRoles(List<MvcController> controllers)
        {
            return RoleManagerBso.SaveActionRoles(controllers);
        }
    }
}
