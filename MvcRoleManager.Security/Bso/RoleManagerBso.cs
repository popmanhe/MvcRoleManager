using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.DAL;
using MvcRoleManager.Models;
using MvcRoleManager.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Bso
{
  public  class RoleManagerBso
    {
        private UnitOfWork unitOfWork = new UnitOfWork(ApplicationDbContext.Create());

        public List<MvcController> GetControllers()
        {
             ControllersActions _controllersActions = new ControllersActions();
       
            return _controllersActions.GetControllers(true);
        }

        public List<IdentityRole> GetRoles()
        {
            var roles = unitOfWork.Repository<IdentityRole>().Get().OrderBy(r => r.Name);
            return roles.ToList();
        }

        public Task<bool> SaveActionPermissions(List<MvcController> controllers)
        {
            throw new NotImplementedException();
        }
    }
}
