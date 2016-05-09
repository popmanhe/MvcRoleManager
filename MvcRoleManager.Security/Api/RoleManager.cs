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

namespace MvcRoleManager.Security.Api
{
  [RoutePrefix("api/rolemanager")        ]
    public class RoleManagerController: ApiController
    {
        private ControllersActions _controllersActions = new ControllersActions ();
       
        private UnitOfWork unitOfWork = new UnitOfWork(ApplicationDbContext.Create()); 
      [AllowAnonymous]
        public List<MvcController> GetControllers() {
            return this._controllersActions.GetControllers(true);
        }

        [AllowAnonymous]
        public List<IdentityRole> GetRoles()
        {
            var roles = unitOfWork.Repository<IdentityRole>().Get().OrderBy(r=>r.Name);
            return roles.ToList();
        }

 [AllowAnonymous]
        public Task<bool> SaveActionPermissions()
        {
            return null;
        }
    }
}
