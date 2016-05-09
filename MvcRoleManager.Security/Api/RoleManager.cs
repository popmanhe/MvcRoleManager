using MvcRoleManager.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using MvcRoleManager.DAL;
using System.Web;
using MvcRoleManager.Models;
using Microsoft.AspNet.Identity.Owin;
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
        public Task<bool> SaveActionPermissions(List<MvcController> controllers)
        {
            return RoleManagerBso.SaveActionPermissions(controllers);
        }
    }
}
