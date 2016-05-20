using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.Security.BSO;
using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.Attributes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
        [HttpGet]
        public List<IdentityRole> GetRoles()
        {
            return RoleManagerBso.GetRoles();
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<int> SaveActionRoles(List<MvcAction> actions)
        {
            return RoleManagerBso.SaveActionRoles(actions);
        }
    }
}
